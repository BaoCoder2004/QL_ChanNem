using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class ChiTietHdn
{
    [Display(Name = "Mã hoá đợn nhập")]
    public int MaHdn { get; set; }

    public string MaChiTietHang { get; set; } = null!;
    [Display(Name = "Số lượng")]
    public int SoLuong { get; set; }
    [Display(Name = "Giá trị")]
    public decimal? ThanhTien { get; set; }

    public virtual ChiTietHang MaChiTietHangNavigation { get; set; } = null!;

    public virtual HoaDonNhap MaHdnNavigation { get; set; } = null!;
}
