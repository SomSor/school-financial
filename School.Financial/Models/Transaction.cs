using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class Transaction : DbModelBase
    {
        [Display(Name = "วันที่ลงรายการ")]
        public DateTime IssueDate { get; set; }
        [Display(Name = "เลขที่ใบสำคัญ")]
        public string DuplicatePaymentType { get; set; }
        public string DuplicatePaymentNumber { get; set; }
        public string DuplicatePaymentYear { get; set; }
        public int? DuplicatePaymentCount { get; set; }
        [Display(Name = "รายการ")]
        public string Title { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string Remark { get; set; }
        [Display(Name = "ผู้รับเงิน")]
        public int? PartnerId { get; set; }
        /// <summary>
        /// + -
        /// </summary>
        [Display(Name = "จำนวน")]
        public decimal Amount { get; set; }
        [Display(Name = "ประเภทรายจ่าย")]
        public string PaymentType { get; set; }
        [Display(Name = "ภาษีมูลค่าเพิ่ม")]
        public decimal? VatInclude { get; set; }
        [Display(Name = "ประเภทสินค้า")]
        public string ProductType { get; set; }
        [Display(Name = "ประเภทงบประมาณ")]
        public int BudgetId { get; set; }
        public int SchoolId { get; set; }
        public int IncomeReceiptId { get; internal set; }

        public string IssueDateString { get { return IssueDate.ToString(WebConfiguration.DateTimeFormat); } }
        public string DuplicatePaymentString
        {
            get
            {
                if (PaymentType == Models.PaymentType.DuplicatePayment || PaymentType == Models.PaymentType.Debtor)
                {
                    var numbers = DuplicatePaymentCount > 1 ? $"-{int.Parse(DuplicatePaymentNumber ?? "1") + DuplicatePaymentCount - 1}" : string.Empty;
                    return $"{DuplicatePaymentType}{DuplicatePaymentNumber}{numbers}/{DuplicatePaymentYear}";
                }
                else
                {
                    return $"{DuplicatePaymentType}{DuplicatePaymentNumber}";
                }
            }
        }
        public string AmountString { get { return Amount.ToString(WebConfiguration.MoneyFormat); } }
        public string AmountPositiveString { get { return Math.Abs(Amount).ToString(WebConfiguration.MoneyFormat); } }
        public decimal TotalAmount { get { return Amount + (VatInclude ?? 0); } }
        public string AmountAbsString { get { return Math.Abs(Amount).ToString(WebConfiguration.MoneyFormat); } }
        public string VatIncludeString { get { return VatInclude.HasValue ? Math.Abs(VatInclude.Value).ToString(WebConfiguration.MoneyFormat) : "-"; } }
        public string TotalAmountString { get { return Math.Abs(TotalAmount).ToString(WebConfiguration.MoneyFormat); } }
        public bool IsPayment { get { return Amount < 0; } }
    }

    public class PaymentType
    {
        /// <summary>
        /// ลูกหนี้
        /// </summary>
        public const string Debtor = nameof(Debtor);
        /// <summary>
        /// ใบสำคัญ
        /// </summary>
        public const string DuplicatePayment = nameof(DuplicatePayment);
    }
}
