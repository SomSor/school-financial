using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class UserInfo : DbModelBase
    {
        [Display(Name = "ชื่อผู้ใช้")]
        public string Username { get; set; }
        [Display(Name = "รหัสผ่าน"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ชื่อ")]
        public string Name { get; set; }
        [Display(Name = "ตำแหน่ง")]
        public string Position { get; set; }
        public string Role { get; set; }
    }
}
