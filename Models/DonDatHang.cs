using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class DonDatHang
{
    [Display(Name = "Mã đơn đặt hàng")]
    public int MaDdh { get; set; }
    
    public string? MaNv { get; set; }

    public string? MaKh { get; set; }
    [Display(Name = "Ngày tạo")]
    public DateOnly? NgayMua { get; set; }
    [Display(Name = "Giá trị")]
    public decimal? TongTien { get; set; }
    [Display(Name = "Trạng thái")]
    public bool DaXuLy { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
