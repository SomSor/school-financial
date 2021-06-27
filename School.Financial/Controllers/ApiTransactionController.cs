using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using School.Financial.Services;
using System;
using System.Linq;

namespace School.Financial.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiTransactionController : ControllerBase
    {
        private readonly ITransactionDac transactionDac;
        private readonly IIncomeReceiptDac incomeReceiptDac;
        private readonly IBringForwardDac bringForwardDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        private SchoolConfig _currentSchoolConfig { get; set; }
        public SchoolConfig CurrentSchoolConfig { get { return _currentSchoolConfig ??= identityService.GetConfig(); } }

        public ApiTransactionController(
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
        }

        /// <summary>
        /// สร้างรายการรายรับ
        /// </summary>
        /// <param name="request">รายรับ</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateIncome(ApiModels.TransactionIncomeRequest request)
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

            return Ok();
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
