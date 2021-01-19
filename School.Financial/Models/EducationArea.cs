using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class EducationArea 
    {
        public string sao_id { get; set; }
        public string sao_type { get; set; }
        [Display(Name = "พื้นที่การศึกษา")]
        public string sao_name { get; set; }
    }
}
