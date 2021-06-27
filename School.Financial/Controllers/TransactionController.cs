using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Financial.ApiModels;
using School.Financial.Dac;
using School.Financial.Helpers;
using School.Financial.Models;
using School.Financial.Services;
using System;
using System.Linq;

namespace School.Financial.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly IBudgetDac budgetDac;
        private readonly IPartnerDac partnerDac;
        private readonly ITransactionDac transactionDac;
        private readonly IIncomeReceiptDac incomeReceiptDac;
        private readonly IBringForwardDac bringForwardDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        private SchoolConfig _currentSchoolConfig { get; set; }
        public SchoolConfig CurrentSchoolConfig { get { return _currentSchoolConfig ??= identityService.GetConfig(); } }

        public TransactionController(
            IBudgetDac budgetDac,
            IPartnerDac partnerDac,
            ITransactionDac transactionDac,
            IIncomeReceiptDac incomeReceiptDac,
            IBringForwardDac bringForwardDac,
            IIdentityService identityService
            )
        {
            this.transactionDac = transactionDac;
            this.incomeReceiptDac = incomeReceiptDac;
            this.bringForwardDac = bringForwardDac;
            this.identityService = identityService;
            this.budgetDac = budgetDac;
            this.partnerDac = partnerDac;
        }

        public IActionResult Index()
        {
            var transactions = transactionDac.Get().Where(x => x.SchoolId == CurrentSchoolData.sc_id).ToList();
            var budgets = budgetDac.Get().ToList();
            budgets.Add(new Budget
            {
                Id = 0,
                Name = "ภาษี ณ ที่จ่าย",
            });
            var vatTransactions = transactions.Where(x => x.VatInclude.HasValue && x.VatInclude > 0).ToList();
            var partners = partnerDac.Get();
            transactions.AddRange(vatTransactions.Select(x => new Transaction
            {
                Id = x.Id,
                IssueDate = x.IssueDate,
                Title = $"รับเงินภาษี ณ ที่จ่ายจาก {partners.FirstOrDefault(p => p.Id == x.PartnerId)?.Name}",
                Amount = x.VatInclude.Value,
            }));
            ViewBag.budgets = budgets.OrderBy(x => x.Name);
            return View(transactions.OrderBy(x => x.IssueDate).ThenBy(x => x.DuplicatePaymentNumber));
        }

        public IActionResult Details(int id)
        {
            var transaction = transactionDac.Get(id);
            var budget = budgetDac.Get(transaction.BudgetId);
            var partner = transaction.PartnerId.HasValue ? partnerDac.Get(transaction.PartnerId.Value) : null;
            ViewBag.budget = budget;
            ViewBag.partner = partner;
            return View(transaction);
        }

        public IActionResult CreateIncome()
        {
            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            ViewBag.budgets = budgets;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateIncome(TransactionIncomeRequest request)
        {
            var txId = incomeReceiptDac.Insert(new IncomeReceipt
            {
                IssueDate = request.IssueDate,
                ReceiveFrom = request.ReceiveFrom,
                Amount = request.Incomes?.Sum(x => Math.Abs(x.Amount)) ?? 0,
                Remark = request.Remark,
            });

            foreach (var income in request.Incomes)
            {
                transactionDac.InsertPayment(new Transaction
                {
                    BudgetId = income.BudgetId,
                    IssueDate = request.IssueDate,
                    DuplicatePaymentType = request.DuplicatePaymentType,
                    DuplicatePaymentNumber = request.DuplicatePaymentNumber,
                    Title = income.Title,
                    Remark = request.Remark,
                    Amount = Math.Abs(income.Amount),
                    SchoolId = CurrentSchoolData.sc_id,
                });
                CalculateBringForword(request.IssueDate, income.BudgetId);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreatePayment()
        {
            var budgets = budgetDac.Get().ToList();
            var partners = partnerDac.Get().OrderBy(x => x.Name);
            budgets.Add(new Budget
            {
                Id = 0,
                Name = "ภาษี ณ ที่จ่าย",
            });
            ViewBag.Budets = budgets.OrderBy(x => x.Name);
            ViewBag.Partners = partners;
            ViewBag.SchoolYear = CurrentSchoolConfig.SchoolYear;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="trackVat">on/null</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayment(Transaction request, string trackVat, int DPYCount)
        {
            if (request.PartnerId.HasValue)
            {
                var partner = partnerDac.Get(request.PartnerId.Value);
                request.VatInclude = partner.PartnerType switch
                {
                    PartnerType.Shop => VatHelper.GetShopVatFromFullAmount(request.Amount),
                    PartnerType.Person => VatHelper.GetPersonVatFromFullAmount(request.Amount),
                    _ => 0,
                };
                var lastDuplicatePaymentNumber = transactionDac.GetLastDuplicatePaymentNumber(CurrentSchoolConfig.SchoolYear);
                var nextNumber = int.Parse(lastDuplicatePaymentNumber?.DuplicatePaymentNumber ?? "1") + (lastDuplicatePaymentNumber?.DuplicatePaymentCount ?? 0);
                request.DuplicatePaymentType = partner.IsInternal ? "บค." : "บจ.";
                request.DuplicatePaymentNumber = nextNumber.ToString();
                request.DuplicatePaymentCount = DPYCount < 1 ? 1 : DPYCount;
                request.DuplicatePaymentYear = CurrentSchoolConfig.SchoolYear;
            }
            request.Amount = -Math.Abs(request.Amount);
            request.SchoolId = CurrentSchoolData.sc_id;

            if (request.BudgetId == 0)
            {
                request.DuplicatePaymentType = null;
                request.DuplicatePaymentNumber = null;
                request.DuplicatePaymentCount = null;
                request.DuplicatePaymentYear = null;
                request.PaymentType = null;
                request.PartnerId = null;
            }

            transactionDac.InsertPayment(request);
            CalculateBringForword(request.IssueDate, request.BudgetId);
            CalculateBringForword(request.IssueDate, 0);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var transaction = transactionDac.Get(id);
            var budgets = budgetDac.Get().ToList();
            var partners = partnerDac.Get().OrderBy(x => x.Name);

            budgets.Add(new Budget
            {
                Id = 0,
                Name = "ภาษี ณ ที่จ่าย",
            });

            ViewBag.Budets = budgets.OrderBy(x => x.Name);
            ViewBag.Partners = partners;

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Transaction request)
        {
            var transaction = transactionDac.Get(id);

            transaction.IssueDate = request.IssueDate;
            transaction.DuplicatePaymentType = request.DuplicatePaymentType;
            transaction.DuplicatePaymentNumber = request.DuplicatePaymentNumber;
            transaction.DuplicatePaymentYear = request.DuplicatePaymentYear;
            transaction.Title = request.Title;
            transaction.Remark = request.Remark;
            transaction.ProductType = request.ProductType;

            if (transaction.Amount < 0)
            {
                if (request.PartnerId.HasValue)
                {
                    var partner = partnerDac.Get(request.PartnerId.Value);
                    transaction.VatInclude = partner.PartnerType switch
                    {
                        PartnerType.Shop => VatHelper.GetShopVatFromFullAmount(request.Amount),
                        PartnerType.Person => VatHelper.GetPersonVatFromFullAmount(request.Amount),
                        _ => 0,
                    };
                }
                transaction.Amount = -Math.Abs(request.Amount);

                if (request.BudgetId == 0)
                {
                    request.DuplicatePaymentType = null;
                    request.DuplicatePaymentNumber = null;
                    request.DuplicatePaymentYear = null;
                    request.PaymentType = null;
                    request.PartnerId = null;
                }
            }

            transactionDac.Update(transaction);
            CalculateBringForword(request.IssueDate, request.BudgetId);
            CalculateBringForword(request.IssueDate, 0);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var transaction = transactionDac.Get(id);
            var budget = budgetDac.Get(transaction.BudgetId);
            var partner = transaction.PartnerId.HasValue ? partnerDac.Get(transaction.PartnerId.Value) : null;
            ViewBag.budget = budget;
            ViewBag.partner = partner;
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Transaction request)
        {
            var transaction = transactionDac.Get(id);
            transactionDac.Delete(id);
            CalculateBringForword(transaction.IssueDate, transaction.BudgetId);
            CalculateBringForword(transaction.IssueDate, 0);

            return RedirectToAction(nameof(Index));
        }

        private void CalculateBringForword(DateTime month, int budgetId)
        {
            var date1 = DateTime.UtcNow;
            var monthCount = ((date1.Year - month.Year) * 12) + date1.Month - month.Month;
            for (int i = 0; i < monthCount; i++)
            {
                var currentMonth = month.AddMonths(i);

                var bringForwordThisMonth = bringForwardDac.Get(currentMonth, budgetId);
                if (bringForwordThisMonth == null)
                {
                    bringForwordThisMonth = new BringForward
                    {
                        Month = currentMonth,
                        BudgetId = budgetId,
                    };
                }
                var bringForwordNextMonth = bringForwardDac.Get(currentMonth.AddMonths(1), budgetId);
                if (bringForwordNextMonth == null)
                {
                    bringForwordNextMonth = new BringForward
                    {
                        Month = currentMonth.AddMonths(1),
                        BudgetId = budgetId,
                    };
                }
                var transactions = transactionDac.Get(currentMonth, budgetId);

                var sumAmount = bringForwordThisMonth.Amount + transactions.Sum(x => x.Amount);
                bringForwordNextMonth.Amount = sumAmount;

                bringForwardDac.Upsert(bringForwordNextMonth);
            }
        }
    }
}
