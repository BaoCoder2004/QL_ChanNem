using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class HoaDonNhap
{
    [Display(Name = "Mã hoá đơn nhập")]
    public int MaHdn { get; set; }

    public string? MaNv { get; set; }
    [Display(Name = "Ngày nhập")]
    public DateOnly? NgayNhap { get; set; }

    public string? MaNcc { get; set; }
    [Display(Name = "Giá trị")]
    public decimal? TongTien { get; set; }

    public virtual NhaCungCap? MaNccNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
