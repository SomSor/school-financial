using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class EducationArea : DbModelBase
    {
        [Display(Name = "พื้นที่การศึกษา")]
        public string Name { get; set; }
    }
}
