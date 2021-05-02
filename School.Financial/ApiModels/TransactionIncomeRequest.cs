using System;
using System.Collections.Generic;

namespace School.Financial.ApiModels
{
    /// <summary>
    /// รายรับ
    /// </summary>
    public class TransactionIncomeRequest
    {
        /// <summary>
        /// ประเภทงบประมาณ
        /// </summary>
        public int BudgetId { get; set; }
        /// <summary>
        /// วันที่ลงรายการ
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// ประเภทใบสำคัญ บ.ร / ไม่ระบุ
        /// </summary>
        public string DuplicatePaymentType { get; set; }
        /// <summary>
        /// เลขที่ใบสำคัญ
        /// </summary>
        public string DuplicatePaymentNumber { get; set; }
        /// <summary>
        /// รายการ
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// จำนวนเงิน
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// โรงเรียน
        /// </summary>
        public int SchoolId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IncomeDetailRequest> Incomes { get; set; }
    }

    public class IncomeDetailRequest
    {
        public string Title { get; set; }
        public string Amount { get; set; }
    }
}
