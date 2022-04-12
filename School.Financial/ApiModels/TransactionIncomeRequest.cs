using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.ApiModels
{
    /// <summary>
    /// รายรับ
    /// </summary>
    public class TransactionIncomeRequest
    {
        /// <summary>
        /// วันที่ลงรายการ
        /// </summary>
        [Required]
        [Display(Name = "วันที่ลงรายการ")]
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// ได้รับเงินจาก
        /// </summary>
        [Required]
        [Display(Name = "ได้รับเงินจาก")]
        public string ReceiveFrom { get; set; }
        /// <summary>
        /// ประเภทใบสำคัญ บ.ร / ไม่ระบุ
        /// </summary>
        [Display(Name = "เลขที่ใบสำคัญ")]
        public string DuplicatePaymentType { get; set; }
        /// <summary>
        /// เลขที่ใบสำคัญ
        /// </summary>
        public string DuplicatePaymentNumber { get; set; }
        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// รายละเอียด
        /// </summary>
        [Display(Name = "รายละเอียด")]
        public IEnumerable<IncomeDetailRequest> Incomes { get; set; }
    }

    public class IncomeDetailRequest
    {
        /// <summary>
        /// ประเภทงบประมาณ
        /// </summary>
        [Required]
        [Display(Name = "ประเภทงบประมาณ")]
        public int BudgetId { get; set; }
        /// <summary>
        /// รายการ
        /// </summary>
        [Required]
        [Display(Name = "รายการ")]
        public string Title { get; set; }
        /// <summary>
        /// จำนวนเงิน
        /// </summary>
        [Required]
        [Display(Name = "จำนวน")]
        public decimal Amount { get; set; }
    }
}
