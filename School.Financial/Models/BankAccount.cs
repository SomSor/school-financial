using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class BankAccount : DbModelBase
    {
        [Display(Name = "ธนาคาร")]
        public string BankName { get; set; }
        [Display(Name = "ชื่อบัญชี")]
        public string AccountName { get; set; }
        [Display(Name = "เลขบัญชี")]
        public string AccountNumber { get; set; }
    }
}
