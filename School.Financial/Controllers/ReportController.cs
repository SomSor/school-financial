using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Helpers;
using School.Financial.Models;
using School.Financial.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

            var FileName = Path.Combine(Directory.GetCurrentDirectory(), "ReportSrc/overallreport.xlsx");

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(FileName, LoadOptions.XlsxDefault);
            var worksheet = workbook.Worksheets[0];

            worksheet.Cells[0, Col.A].SetValue(month.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture("th-TH")));
            worksheet.Cells[0, Col.C].SetValue(CurrentSchoolData.Name);
            worksheet.Cells[2, Col.C].SetValue(month.Value.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));

            var bankSumTotal = (double)transactions.Sum(t => t.Amount);
            worksheet.Cells[5, Col.D].SetValue(0);
            worksheet.Cells[5, Col.E].SetValue(bankSumTotal);
            worksheet.Cells[5, Col.F].SetValue(0);
            worksheet.Cells[5, Col.G].SetValue(bankSumTotal);

            var issuer = identityService.GetUser();
            worksheet.Cells[7, Col.D].SetValue(issuer.Name);
            worksheet.Cells[8, Col.D].SetValue($"ตำแหน่ง เจ้าหน้าที่การเงิน{CurrentSchoolData.Name}");
            worksheet.Cells[14, Col.D].SetValue("นายสุขสันต์ สอนนวล");
            worksheet.Cells[15, Col.D].SetValue($"ผู้อำนวยการ{CurrentSchoolData.Name}");
            worksheet.Cells[17, Col.A].SetValue($"ข้าพเจ้า/ผู้รับมอบหมายได้รับเงินสดตามรายการข้างต้นแล้ว เมื่อวันที่ {month.Value.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH"))}");

            var tableRowIndex = 5;
            var currentRowIndex = tableRowIndex;
            foreach (var item in budgets)
            {
                worksheet.Rows[currentRowIndex].Style.NumberFormat = NumberFormatBuilder.Accounting(2);

                worksheet.Rows.InsertCopy(currentRowIndex, worksheet.Rows[currentRowIndex]);
                var bankSum = (double)transactions.Where(t => t.BudgetId == item.Id)
                    .OrderBy(x => x.IssueDate)
                    .ThenBy(x => x.Id)
                    .ToList()
                    .Sum(t => t.Amount);

                worksheet.Cells[currentRowIndex, Col.A].SetValue(item.Name);
                worksheet.Cells[currentRowIndex, Col.D].SetValue(0);
                worksheet.Cells[currentRowIndex, Col.E].SetValue(bankSum);
                worksheet.Cells[currentRowIndex, Col.F].SetValue(0);
                worksheet.Cells[currentRowIndex, Col.G].SetValue(bankSum);

                worksheet.Cells[currentRowIndex, Col.A].Style.Borders[IndividualBorder.Top | IndividualBorder.Bottom].LineStyle = LineStyle.None;
                worksheet.Cells[currentRowIndex, Col.A].Style.Borders[IndividualBorder.Left | IndividualBorder.Right].LineStyle = LineStyle.Thin;
                worksheet.Cells[currentRowIndex, Col.D].Style = worksheet.Cells[currentRowIndex, Col.A].Style;
                worksheet.Cells[currentRowIndex, Col.E].Style = worksheet.Cells[currentRowIndex, Col.A].Style;
                worksheet.Cells[currentRowIndex, Col.F].Style = worksheet.Cells[currentRowIndex, Col.A].Style;
                worksheet.Cells[currentRowIndex, Col.G].Style = worksheet.Cells[currentRowIndex, Col.A].Style;

                currentRowIndex++;
            }

            worksheet.Cells.GetSubrange($"A{tableRowIndex}", $"G{tableRowIndex + budgets.Count()}").Style.Borders[IndividualBorder.Top | IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
            worksheet.Cells.GetSubrange($"D{tableRowIndex + budgets.Count() + 1}", $"G{tableRowIndex + budgets.Count() + 1}").Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thick;

            var contentStream = new MemoryStream();
            worksheet.PrintOptions.PaperType = PaperType.A4;
            workbook.Save(contentStream, SaveOptions.PdfDefault);

            return File(contentStream, "application/pdf", "OverAllReport.pdf");
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
            var worksheet = workbook.Worksheets[0];

            worksheet.Cells[2, 17].SetValue(1);
            worksheet.Cells[3, 17].SetValue(transaction.Id.ToString());
            worksheet.Cells[5, 3].SetValue(CurrentSchoolData.Name);
            worksheet.Cells[5, 17].SetValue(CurrentSchoolData.VatId);
            worksheet.Cells[7, 3].SetValue(CurrentSchoolData.Address);
            worksheet.Cells[11, 3].SetValue(transaction.Partner.Name);
            worksheet.Cells[13, 3].SetValue(transaction.Partner.Address);
            if (transaction.Partner.PartnerType == PartnerType.Person)
            {
                worksheet.Cells[10, 17].SetValue(transaction.Partner.VatNumber);
                worksheet.Cells[11, 17].SetValue(string.Empty);
            }
            else
            {
                worksheet.Cells[10, 17].SetValue(string.Empty);
                worksheet.Cells[11, 17].SetValue(transaction.Partner.VatNumber);
            }
            worksheet.Cells[42, 6].SetValue(transaction.ProductType);
            worksheet.Cells[42, 12].SetValue(transaction.IssueDate.ToString("d MMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            worksheet.Cells[42, 14].SetValue((double)(Math.Abs(transaction.Amount) - transaction.VatInclude.Value));
            worksheet.Cells[42, 16].SetValue((double)transaction.VatInclude.Value);
            worksheet.Cells[48, 14].SetValue((double)(Math.Abs(transaction.Amount) - transaction.VatInclude.Value));
            worksheet.Cells[48, 16].SetValue((double)transaction.VatInclude.Value);
            worksheet.Cells[50, 8].SetValue(VatHelper.ThaiBaht(transaction.VatInclude.ToString()));
            worksheet.Cells[58, 10].SetValue(transaction.IssueDate.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            worksheet.Cells[62, 12].SetValue(transaction.IssueDate.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH")));
            worksheet.Cells[64, 5].SetValue(transaction.Partner.Name);
            worksheet.Cells[66, 6].SetValue(VatHelper.ThaiBaht((Math.Abs(transaction.Amount) - transaction.VatInclude).ToString()));
            worksheet.Cells[67, 13].SetValue((double)(Math.Abs(transaction.Amount) - transaction.VatInclude.Value));

            var contentStream = new MemoryStream();
            worksheet.PrintOptions.PaperType = PaperType.A4;
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
