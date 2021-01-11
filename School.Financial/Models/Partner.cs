using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class Partner : DbModelBase
    {
        [Display(Name = "ชื่อ")]
        public string Name { get; set; }
        [Display(Name = "เลขประจำตัวประชาชน/ผู้เสียภาษี")]
        public string VatNumber { get; set; }
        [Display(Name = "ที่อยู่")]
        public string Address { get; set; }
        [Display(Name = "ประเภท")]
        public string PartnerType { get; set; }
        [Display(Name = "บุคคลากรภายใน")]
        public bool IsInternal { get; set; }

        public string PartnerTypeString => PartnerType switch
        {
            Models.PartnerType.Normal => "ไม่ระบุ",
            Models.PartnerType.Person => "นิติบุคคล",
            Models.PartnerType.Shop => "ร้านค้า",
            _ => string.Empty,
        };
    }

    public class PartnerType
    {
        public const string Normal = nameof(Normal);
        public const string Person = nameof(Person);
        public const string Shop = nameof(Shop);
    }
}
