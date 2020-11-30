using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class Transaction : DbModelBase
    {
        /// <summary>
        /// วันที่ลงรายการ
        /// </summary>
        [Display(Name = "วันที่ลงรายการ")]
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// เลขที่ใบสำคัญคู่จ่าย
        /// </summary>
        [Display(Name = "เลขที่ใบสำคัญคู่จ่าย")]
        public string PayEvidence { get; set; }
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
        [Display(Name = "มีภาษีมูลค่าเพิ่ม")]
        public bool? IsTrackVat { get; set; }
        [Display(Name = "ภาษีมูลค่าเพิ่ม")]
        public decimal? VatInclude { get; set; }

        /// <summary>
        /// เงินคงเหลือธนาคาร
        /// </summary>
        [Display(Name = "เงินคงเหลือธนาคาร")]
        public decimal Remain { get; set; }
        /// <summary>
        /// เงินสดคงเหลือ
        /// </summary>
        [Display(Name = "เงินสดคงเหลือ")]
        public decimal Cash { get; set; }
        /// <summary>
        /// เงินฝาก
        /// </summary>
        [Display(Name = "เงินฝาก")]
        public decimal Deposit { get; set; }

        [Display(Name = "ประเภทงบประมาณ")]
        public int BudgetId { get; set; }

        public string IssueDateString { get { return IssueDate.ToString(WebConfiguration.DateTimeFormat); } }
        public string AmountString { get { return Amount.ToString(WebConfiguration.MoneyFormat); } }
        public bool IsPayment { get { return Amount < 0; } }
    }
}
