using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class DanhMucHang
{
    [Required(ErrorMessage ="Vui lòng nhập mã hàng")]
    [MaxLength(20)]
    [Display(Name = "Mã hàng")]
    public string MaHang { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập tên hàng")]
    [MaxLength(300)]
    [Display(Name = "Tên hàng")]
    public string TenHang { get; set; } = null!;

    public string? MaLoai { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mô tả")]
    [Display(Name = "Mô tả")]
    public string? ThongTin { get; set; }

    public int? ThoiGianBaoHanh { get; set; }
    [Display(Name = "Trạng thái")]
    public bool NgungKinhDoanh { get; set; }

    public virtual ICollection<ChiTietHang> ChiTietHangs { get; set; } = new List<ChiTietHang>();

    public virtual TheLoai? MaLoaiNavigation { get; set; }
}
