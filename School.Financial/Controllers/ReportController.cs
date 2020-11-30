using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace School.Financial.Controllers
{
    public class ReportController : Controller
    {
        private readonly IBudgetDac budgetDac;
        private readonly ITransactionDac transactionDac;
        private readonly IBringForwardDac bringForwardDac;

        public ReportController(
            IBudgetDac budgetDac,
            ITransactionDac transactionDac,
            IBringForwardDac bringForwardDac
            )
        {
            this.budgetDac = budgetDac;
            this.transactionDac = transactionDac;
            this.bringForwardDac = bringForwardDac;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OverAllReport(DateTime? month)
        {
            if (month == null) month = DateTime.UtcNow;
            ViewBag.month = month;

            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            var transactions = transactionDac.Get(month.Value).ToList();
            var bringForwordThisMonths = bringForwardDac.Get(month.Value);
            transactions.InsertRange(0, bringForwordThisMonths.Select(b => new Transaction
            {
                IssueDate = new DateTime(month.Value.Year, month.Value.Month, 1),
                Title = "ยอดยกมา",
                Amount = b?.Amount ?? 0,
                BudgetId = b.BudgetId,
            }));
            var response = new OverAllReport
            {
                Budgets = budgets.OrderBy(x => x.Name).Select(x => new OverAllReportDetail
                {
                    Budget = x,
                    Transactions = transactions.Where(t => t.BudgetId == x.Id).ToList(),
                }).ToList(),
            };
            return View(response);
        }

        public ActionResult OverAllVat(DateTime? month)
        {
            if (month == null) month = DateTime.UtcNow;
            ViewBag.month = month;

            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            ViewBag.budgets = budgets;

            var transactions = transactionDac.GetWithVat(month.Value).ToList();
            return View(transactions);
        }

        public ActionResult TransactionReport(DateTime? month, int budgetId)
        {
            if (month == null) month = DateTime.UtcNow;
            ViewBag.month = month;
            ViewBag.budgetId = budgetId;

            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            ViewBag.budgets = budgets;

            var transactions = transactionDac.Get(month.Value, budgetId).ToList();
            var bringForword = bringForwardDac.Get(month.Value, budgetId);
            if (bringForword != null)
            {
                transactions.Insert(0, new Transaction
                {
                    IssueDate = new DateTime(month.Value.Year, month.Value.Month, 1),
                    Title = "ยอดยกมา",
                    Amount = bringForword?.Amount ?? 0,
                    BudgetId = bringForword.BudgetId,
                });
            }
            return View(transactions);
        }

        public ActionResult VatReport(int month)
        {
            var transactions = transactionDac.Get();
            return View(transactions);
        }
    }
}
namespace School.Financial.Models
{
    public class OverAllReport
    {
        public IEnumerable<OverAllReportDetail> Budgets { get; set; }
    }

    public class OverAllReportDetail
    {
        public Budget Budget { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
