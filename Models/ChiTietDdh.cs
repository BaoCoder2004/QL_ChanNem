using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class ChiTietDdh
{
    [Key]
    [Display(Name = "Mã đơn đặt hàng")]
    public int MaDdh { get; set; }

    public string MaChiTietHang { get; set; } = null!;
    [Display(Name = "Số lượng")]
    public int SoLuong { get; set; }
    [Display(Name = "Giảm giá")]
    public int? GiamGia { get; set; }
    [Display(Name = "Trị giá")]
    public decimal? ThanhTien { get; set; }

    public virtual ChiTietHang MaChiTietHangNavigation { get; set; } = null!;

    public virtual DonDatHang MaDdhNavigation { get; set; } = null!;
}
