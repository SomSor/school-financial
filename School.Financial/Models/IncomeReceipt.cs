using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class IncomeReceipt : DbModelBase
    {
        /// <summary>
        /// วันที่ลงรายการ
        /// </summary>
        [Display(Name = "วันที่ลงรายการ")]
        public DateTime IssueDate { get; set; }
        [Display(Name = "ได้รับเงินจาก")]
        public string ReceiveFrom { get; set; }
        public string Remark { get; set; }
        public decimal Amount { get; set; }
    }
}
