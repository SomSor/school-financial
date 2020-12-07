using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class Budget : DbModelBase
    {
        /// <summary>
        /// ประเภท
        /// </summary>
        [Display(Name = "ประเภท")]
        public string Name { get; set; }
        public string BankAccountId { get; set; }
    }
}
