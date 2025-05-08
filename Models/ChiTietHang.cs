using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class ChiTietHang
{
    [Display(Name = "Mã chi tiết hàng")]
    public string MaChiTietHang { get; set; } = null!;

    public string MaHang { get; set; } = null!;
    [Display(Name = "Màu sắc")]
    public string? MaMau { get; set; }
    [Display(Name = "Chất liệu")]
    public string? MaChatLieu { get; set; }
    [Display(Name = "Kích thước")]
    public string? MaKichThuoc { get; set; }
    [Display(Name = "Giá nhập")]
    public decimal? DonGiaNhap { get; set; }
    [Display(Name = "Giá bán")]
    public decimal? DonGiaBan { get; set; }
    [Display(Name = "Số lượng")]
    public int SoLuong { get; set; }

    public virtual ICollection<AnhHang> AnhHangs { get; set; } = new List<AnhHang>();

    public virtual ChatLieu? MaChatLieuNavigation { get; set; }

    public virtual DanhMucHang? MaHangNavigation { get; set; }

    public virtual KichThuoc? MaKichThuocNavigation { get; set; }

    public virtual MauSac? MaMauNavigation { get; set; }
}
