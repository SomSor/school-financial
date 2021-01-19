using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class SchoolData
    {
        public int sc_id { get; set; }
        public int smis { get; set; }
        public int sc_code { get; set; }
        [Display(Name = "โรงเรียน")]
        public string sc_name { get; set; }
        [Display(Name = "พื้นที่การศึกษา")]
        public string areacode { get; set; }
        public int sc_network { get; set; }
        public string aumphur { get; set; }
        public string province { get; set; }
        public string spt { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string tumbol { get; set; }
        public int p_code { get; set; }
        public string tel { get; set; }
        public string low_class { get; set; }
        public string top_class { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string VatId { get; set; }
        [Display(Name = "ที่อยู่")]
        public string Address { get => $"{add2} หมู่ {add1} {sc_name} ต.{tumbol} อ.{aumphur} จ.{province}"; }
        //[Display(Name = "เลขประจำตัวผู้เสียภาษีอากร")]
        //public string VatId { get; set; }
    }
}
