using BTL_TKWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.ModelView
{
    public class ChiTietHangVaAnh
    {
        [Display(Name = "Mã chi tiết hàng")]
        public string MaChiTietHang { get; set; } = null!;
        [Required]
        public string MaHang { get; set; } = null!;
        [Display(Name = "Màu sắc")]
        public string? MaMau { get; set; }
        [Display(Name = "Chất liệu")]
        public string? MaChatLieu { get; set; }
        [Display(Name = "Kích thước")]
        public string? MaKichThuoc { get; set; }
        [Required]
        [DataType(DataType.Currency,ErrorMessage ="Vui lòng nhập đúng giá trị")]
        [Range(0, double.MaxValue, ErrorMessage = "Vui lòng nhập đúng giá trị")]
        [Display(Name = "Giá nhập")]
        public decimal? DonGiaNhap { get; set; }
        [Required]
        [DataType(DataType.Currency, ErrorMessage = "Vui lòng nhập đúng giá trị")]
        [Range(0, double.MaxValue, ErrorMessage = "Vui lòng nhập đúng giá trị")]
        [Display(Name = "Giá bán")]
        public decimal? DonGiaBan { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập đúng giá trị")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        [Display(Name ="Ảnh")]
        public IFormFile? Anh { set; get; }
        public virtual ICollection<AnhHang> AnhHangs { get; set; } = new List<AnhHang>();

        public virtual ChatLieu? MaChatLieuNavigation { get; set; }

        public virtual DanhMucHang? MaHangNavigation { get; set; }

        public virtual KichThuoc? MaKichThuocNavigation { get; set; }

        public virtual MauSac? MaMauNavigation { get; set; }
    }
}
