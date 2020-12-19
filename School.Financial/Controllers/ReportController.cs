using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using School.Financial.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace School.Financial.Controllers
{
    public class ReportController : Controller
    {
        private readonly IBudgetDac budgetDac;
        private readonly IPartnerDac partnerDac;
        private readonly ITransactionDac transactionDac;
        private readonly IBringForwardDac bringForwardDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        public ReportController(
            IBudgetDac budgetDac,
            IPartnerDac partnerDac,
            ITransactionDac transactionDac,
            IBringForwardDac bringForwardDac,
            IIdentityService identityService
            )
        {
            this.budgetDac = budgetDac;
            this.partnerDac = partnerDac;
            this.transactionDac = transactionDac;
            this.bringForwardDac = bringForwardDac;
            this.identityService = identityService;
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
                    Transactions = transactions.Where(t => t.BudgetId == x.Id).OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList(),
                }).ToList(),
            };
            return View(response);
        }

        public ActionResult OverAllReportFile(DateTime? month)
        {
            if (month == null) month = DateTime.UtcNow;

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
                    Transactions = transactions.Where(t => t.BudgetId == x.Id).OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList(),
                }).ToList(),
            };

            var content = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "ReportSrc/overallreport.xml"));

            var tableRowIndex = content.IndexOf("{tablerow-title}");
            var startRowIndex = content.Substring(0, tableRowIndex).LastIndexOf("<Row");
            var endRowIndex = content.IndexOf("</Row>", tableRowIndex) + 6;

            var rowContentRaw = content.Substring(startRowIndex, endRowIndex - startRowIndex);

            content = content.Replace("{month}", month.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture("th-TH")));
            content = content.Replace("{schoolname}", CurrentSchoolData.Name);
            content = content.Replace("{date}", month.Value.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));

            var RefersToText = "RefersTo=\"=Sheet1!R1C1:R";
            var RefersToStartIndex = content.IndexOf(RefersToText) + RefersToText.Length;
            var RefersToEndIndex = content.IndexOf("\"", RefersToStartIndex);
            var RefersTo = content.Substring(RefersToStartIndex, RefersToEndIndex - RefersToStartIndex).Split("C");

            var oldVal = $"{RefersToText}{RefersTo[0]}C{RefersTo[1]}\"";
            var newVal = $"{RefersToText}{int.Parse(RefersTo[0]) + budgets.Count()}C{RefersTo[1]}\"";
            content = content.Replace(oldVal, newVal);

            var expandedRowCountText = "ExpandedRowCount=\"";
            var expandedRowCountStartIndex = content.IndexOf(expandedRowCountText) + expandedRowCountText.Length;
            var expandedRowCountEndIndex = content.IndexOf("\"", expandedRowCountStartIndex);
            var expandedRowCount = content.Substring(expandedRowCountStartIndex, expandedRowCountEndIndex - expandedRowCountStartIndex);

            oldVal = $"{expandedRowCountText}{expandedRowCount}\"";
            newVal = $"{expandedRowCountText}{int.Parse(expandedRowCount) + budgets.Count()}\"";
            content = content.Replace(oldVal, newVal);

            var rowContent = string.Empty;
            foreach (var item in response.Budgets)
            {
                var bankSum = item.Transactions.Sum(t => t.Amount).ToString();
                rowContent += rowContentRaw
                    .Replace("{tablerow-title}", item.Budget.Name)
                    .Replace("Type=\"String\">{tablerow-cach}", "Type=\"Number\">0")
                    .Replace("Type=\"String\">{tablerow-bank}", $"Type=\"Number\">{bankSum}")
                    .Replace("Type=\"String\">{tablerow-deposit}", "Type=\"Number\">0")
                    .Replace("Type=\"String\">{tablerow-total}", $"Type=\"Number\">{bankSum}");
            }
            content = content.Replace(rowContentRaw, rowContent);

            var bankSumTotal = transactions.Sum(t => t.Amount).ToString();
            content = content.Replace("Type=\"String\">{total-cash}", "Type=\"Number\">0");
            content = content.Replace("Type=\"String\">{total-bank}", $"Type=\"Number\">{bankSumTotal}");
            content = content.Replace("Type=\"String\">{total-deposit}", "Type=\"Number\">0");
            content = content.Replace("Type=\"String\">{total}", $"Type=\"Number\">{bankSumTotal}");

            return File(Encoding.UTF8.GetBytes(content), "application/vnd.ms-excel", "OverAllReport.xls");
        }

        public ActionResult OverAllVatReport(DateTime? month)
        {
            if (month == null) month = DateTime.UtcNow;
            ViewBag.month = month;

            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            ViewBag.budgets = budgets;

            var transactions = transactionDac.GetWithVat(month.Value).OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList();
            return View(transactions);
        }

        public ActionResult TransactionReport(DateTime? month, int budgetId = -1)
        {
            if (month == null) month = DateTime.UtcNow;
            ViewBag.month = month;
            ViewBag.budgetId = budgetId;

            var budgets = budgetDac.Get().ToList();
            budgets.Add(new Budget
            {
                Id = 0,
                Name = "ภาษี ณ ที่จ่าย",
            });
            ViewBag.budgets = budgets.OrderBy(x => x.Name);

            var partners = partnerDac.Get();
            var transactions = budgetId switch
            {
                0 => transactionDac.GetTeackVat(month.Value).OrderBy(x => x.IssueDate).ThenBy(x => x.Id).Select(x => new Transaction
                {
                    Id = x.Id,
                    IssueDate = x.IssueDate,
                    Title = $"รับเงินภาษี ณ ที่จ่ายจาก {partners.FirstOrDefault(p => p.Id == x.PartnerId)?.Name}",
                    Amount = x.VatInclude.Value,
                }).ToList(),
                _ => transactionDac.Get(month.Value, budgetId).OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList(),
            };
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
            var transactions = transactionDac.Get().OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList();
            return View(transactions);
        }

        public ActionResult ChequeReport()
        {
            var transactions = transactionDac.GetWithPartner().OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList();
            return View(transactions);
        }

        public ActionResult ChequeReportFile(int id)
        {
            var transaction = transactionDac.GetWithPartner(id);
            var FileName = Path.Combine(Directory.GetCurrentDirectory(), "ReportSrc/chequereport.xlsx");

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(FileName, LoadOptions.XlsxDefault);

            workbook.Worksheets[0].Cells[2, 17].SetValue("1");
            workbook.Worksheets[0].Cells[3, 17].SetValue(transaction.Id.ToString());
            workbook.Worksheets[0].Cells[5, 3].SetValue(CurrentSchoolData.Name);
            workbook.Worksheets[0].Cells[5, 17].SetValue(CurrentSchoolData.VatId);
            workbook.Worksheets[0].Cells[7, 3].SetValue(CurrentSchoolData.Address);
            workbook.Worksheets[0].Cells[11, 3].SetValue(transaction.Partner.Name);
            workbook.Worksheets[0].Cells[13, 3].SetValue(transaction.Partner.Address);
            if (transaction.Partner.PartnerType == PartnerType.Person)
            {
                workbook.Worksheets[0].Cells[10, 17].SetValue(transaction.Partner.VatNumber);
                workbook.Worksheets[0].Cells[11, 17].SetValue(string.Empty);
            }
            else
            {
                workbook.Worksheets[0].Cells[10, 17].SetValue(string.Empty);
                workbook.Worksheets[0].Cells[11, 17].SetValue(transaction.Partner.VatNumber);
            }
            workbook.Worksheets[0].Cells[42, 6].SetValue(transaction.ProductType);
            workbook.Worksheets[0].Cells[42, 12].SetValue(transaction.IssueDate.ToString("d MMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            workbook.Worksheets[0].Cells[42, 14].SetValue((Math.Abs(transaction.Amount) - transaction.VatInclude.Value).ToString("#,##0.00"));
            workbook.Worksheets[0].Cells[42, 16].SetValue(transaction.VatInclude.Value.ToString("#,##0.00"));
            workbook.Worksheets[0].Cells[48, 14].SetValue((Math.Abs(transaction.Amount) - transaction.VatInclude.Value).ToString("#,##0.00"));
            workbook.Worksheets[0].Cells[48, 16].SetValue(transaction.VatInclude.Value.ToString("#,##0.00"));
            workbook.Worksheets[0].Cells[50, 8].SetValue(Helpers.VatHelper.ThaiBaht(transaction.VatInclude.ToString()));
            workbook.Worksheets[0].Cells[58, 10].SetValue(transaction.IssueDate.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            workbook.Worksheets[0].Cells[62, 12].SetValue(transaction.IssueDate.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            workbook.Worksheets[0].Cells[64, 5].SetValue(transaction.Partner.Name);
            workbook.Worksheets[0].Cells[66, 6].SetValue(Helpers.VatHelper.ThaiBaht((Math.Abs(transaction.Amount) - transaction.VatInclude).ToString()));
            workbook.Worksheets[0].Cells[67, 13].SetValue((Math.Abs(transaction.Amount) - transaction.VatInclude.Value).ToString("#,##0.00"));

            var contentStream = new MemoryStream();
            workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A4;
            workbook.Save(contentStream, SaveOptions.PdfDefault);

            return File(contentStream, "application/pdf", "ChequeReport.pdf");
        }

        public ActionResult DuplicatePaymentReport()
        {
            var transactions = transactionDac.GetDuplicatePayment().OrderBy(x => x.IssueDate).ThenBy(x => x.Id).ToList();
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

    public class TransactionWithPartner : Transaction
    {
        public Partner Partner { get; set; }

        public string TotalAmountChequeString { get { return VatInclude.HasValue ? Math.Abs(TotalAmount).ToString(WebConfiguration.MoneyFormat) : "-"; } }
    }
}
