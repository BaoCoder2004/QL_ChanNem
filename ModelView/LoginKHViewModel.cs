using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.ModelView
{
    public class LoginKHViewModel
    {
        [Key]
        [MaxLength(20)]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
        [MaxLength(32)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu có tối thiểu 6 ký tự")]
        public string Password { get; set; }
    }
}
