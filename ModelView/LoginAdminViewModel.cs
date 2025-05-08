using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.ModelView
{
    public class LoginAdminViewModel
    {
        [Key]
        [MaxLength(20)]
        [Required(ErrorMessage ="Vui lòng nhập mã nhân viên")]
        [Display(Name="Mã nhân viên")]
        public string UserName { get; set; }
        [MaxLength(32)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mã nhân viên")]
        [MinLength(6, ErrorMessage ="Mật khẩu có tối thiểu 6 ký tự")]
        public string Password { get; set; }
    }
}
