using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class NhaCungCap
{
    [Display(Name = "Mã NCC")]
    public string MaNcc { get; set; } = null!;
    [Display(Name = "Tên NCC")]
    public string? TenNcc { get; set; }
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }
    [Display(Name = "SĐT")]
    public string? DienThoai { get; set; }

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
}
