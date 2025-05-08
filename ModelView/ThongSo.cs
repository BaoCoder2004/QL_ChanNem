using BTL_TKWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.ModelView
{
    public class ThongSo
    {
        [Display(Name = "Mã kích thước")]
        [MaxLength(20)]
        public string MaKichThuoc { get; set; } = null!;
        [Display(Name = "Kích thước")]
        [MaxLength(50)]
        public string? TenKichThuoc { get; set; }
        [Display(Name = "Mã chất liệu")]
        [MaxLength(20)]
        public string MaChatLieu { get; set; } = null!;
        [Display(Name = "Tên chất liệu")]
        [MaxLength(50)]
        public string? TenChatLieu { get; set; }
        [Display(Name = "Mã màu")]
        [MaxLength(20)]
        public string MaMau { get; set; } = null!;
        [Display(Name = "Màu sắc")]
        [MaxLength(50)]
        public string? TenMau { get; set; }
    }
}
