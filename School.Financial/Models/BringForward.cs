using System;
using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class BringForward : DbModelBase
    {
        [Display(Name = "จำนวน")]
        public decimal Amount { get; set; }
        [Display(Name = "จำนวน")]
        public DateTime Month { get; set; }
        [Display(Name = "ประเภทงบประมาณ")]
        public int BudgetId { get; set; }
    }
}
