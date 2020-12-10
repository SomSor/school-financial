using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class Budget : DbModelBase
    {
        [Display(Name = "ประเภทงบประมาณ")]
        public string Name { get; set; }
        [Display(Name = "บัญชีธนาคาร")]
        public int BankAccountId { get; set; }
        public int SchoolId { get; set; }
    }
}
