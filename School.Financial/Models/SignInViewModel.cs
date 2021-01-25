using System.ComponentModel.DataAnnotations;

namespace School.Financial.Models
{
    public class SignInViewModel
    {
        [Display(Name = "ชื่อผู้ใช้")]
        public string Username { get; set; }
        [Display(Name = "รหัสผ่าน"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
