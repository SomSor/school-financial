using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class SchoolData : DbModelBase
    {
        [Display(Name = "โรงเรียน")]
        public string Name { get; set; }
        [Display(Name = "ที่อยู่")]
        public string Address { get; set; }
        [Display(Name = "เลขประจำตัวผู้เสียภาษีอากร")]
        public string VatId { get; set; }
        [Display(Name = "พื้นที่การศึกษา")]
        public int EducationAreaId { get; set; }
    }
}
