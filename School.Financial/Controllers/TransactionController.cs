using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Helpers;
using School.Financial.Models;
using System;
using System.Linq;

namespace School.Financial.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IBudgetDac budgetDac;
        private readonly IPartnerDac partnerDac;
        private readonly ITransactionDac transactionDac;
        private readonly IBringForwardDac bringForwardDac;

        public TransactionController(
            IBudgetDac budgetDac,
            IPartnerDac partnerDac,
            ITransactionDac transactionDac,
            IBringForwardDac bringForwardDac
            )
        {
            this.transactionDac = transactionDac;
            this.bringForwardDac = bringForwardDac;
            this.budgetDac = budgetDac;
            this.partnerDac = partnerDac;
        }

        public IActionResult Index()
        {
            var transactions = transactionDac.Get().OrderBy(x => x.IssueDate).ToList();
            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            ViewBag.budgets = budgets;
            return View(transactions);
        }

        public IActionResult Details(int id)
        {
            var transaction = transactionDac.Get(id);
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
        public IActionResult CreateIncome(Transaction request)
        {
            request.DuplicatePaymentType= string.Empty;
            request.DuplicatePaymentNumber = string.Empty;
            request.DuplicatePaymentYear = string.Empty;
            request.PartnerId = null;
            request.IsTrackVat = null;
            request.VatInclude = null;
            request.Amount = Math.Abs(request.Amount);

            transactionDac.Insert(request);
            CalculateBringForword(request.IssueDate, request.BudgetId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreatePayment()
        {
            var bidgets = budgetDac.Get().OrderBy(x => x.Name);
            var partners = partnerDac.Get().OrderBy(x => x.Name);
            ViewBag.Budets = bidgets;
            ViewBag.Partners = partners;
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
        public IActionResult CreatePayment(Transaction request, string trackVat)
        {
            request.IsTrackVat = trackVat == "on";
            if (request.IsTrackVat.Value && request.PartnerId.HasValue)
            {
                var partner = partnerDac.Get(request.PartnerId.Value);
                request.VatInclude = partner.PartnerType switch
                {
                    PartnerType.Shop => VatHelper.GetShopVatFromFullAmount(request.Amount),
                    PartnerType.Person => VatHelper.GetPersonVatFromFullAmount(request.Amount),
                    _ => 0,
                };
            }
            request.Amount = -Math.Abs(request.Amount);

            transactionDac.Insert(request);
            CalculateBringForword(request.IssueDate, request.BudgetId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var transaction = transactionDac.Get(id);
            var bidgets = budgetDac.Get().OrderBy(x => x.Name);
            var partners = partnerDac.Get().OrderBy(x => x.Name);
            ViewBag.Budets = bidgets;
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
            transaction.Amount = request.Amount;
            transaction.Remark = request.Remark;

            transactionDac.Update(transaction);
            CalculateBringForword(request.IssueDate, request.BudgetId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var transaction = transactionDac.Get(id);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Transaction request)
        {
            transactionDac.Delete(id);
            CalculateBringForword(request.IssueDate, request.BudgetId);

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
