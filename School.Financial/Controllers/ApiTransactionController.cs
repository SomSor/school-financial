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
        private readonly IBringForwardDac bringForwardDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        private SchoolConfig _currentSchoolConfig { get; set; }
        public SchoolConfig CurrentSchoolConfig { get { return _currentSchoolConfig ??= identityService.GetConfig(); } }

        public ApiTransactionController(
            ITransactionDac transactionDac,
            IBringForwardDac bringForwardDac,
            IIdentityService identityService
            )
        {
            this.transactionDac = transactionDac;
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
            var income = new Transaction
            {
                IssueDate = request.IssueDate,
                DuplicatePaymentType = string.IsNullOrWhiteSpace(request.DuplicatePaymentNumber) ? string.Empty : "บร.",
                DuplicatePaymentNumber = request.DuplicatePaymentNumber,
                DuplicatePaymentYear = string.Empty,
                Title = request.Title,
                Remark = request.Remark,
                DuplicatePaymentCount = 1,
                PartnerId = null,
                VatInclude = null,
                Amount = Math.Abs(request.Amount),
                SchoolId = CurrentSchoolData.sc_id,
            };

            transactionDac.InsertPayment(income);
            CalculateBringForword(request.IssueDate, request.BudgetId);

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
