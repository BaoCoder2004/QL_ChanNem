using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.ModelView
{
    public class RegisterViewModel
    {
        [Key]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Tài khoản có độ dài từ 6 - 32 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Không chứa dấu cách")]
        [Display(Name = "Tài khoản")]
        public string MaKh { get; set; }
        [MaxLength(60)]
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        [Display(Name = "Tên người dùng")]
        public string TenKh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [MaxLength(10)]
        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        [Display(Name = "Điện thoại")]
        public string DienThoai { get; set; }
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu có độ dài từ 6 - 32 ký tự")]
        [RegularExpression(@"^(?=.*\w+)(?=.*[A-Z]+)(?=.*\d+)(?=.*[!#$%&/()=?»«@|£§€{}*.;'<>_,-]+).*$", ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự số, ký tự đặc biệt, chữ cái viết hoa và chữ cái thường")]
        [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
        [DataType(DataType.Password, ErrorMessage = "Nhập đúng định dạng mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Địa chỉ bắt buộc phải được nhập")]
        [MaxLength(250)]
        [Display(Name ="Địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Hãy nhập lại mật khẩu")]
        [Display(Name = "Nhắc lại mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu chưa trùng khớp")]
        public string NhacLaiMatKhau { get; set; }  
    }
}
