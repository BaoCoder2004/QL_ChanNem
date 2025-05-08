using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class KichThuoc
{
    [Display(Name = "Mã kích thước")]
    [Required(ErrorMessage ="Trường này không được bỏ trống")]
    [MaxLength(20)]
    public string MaKichThuoc { get; set; } = null!;
    [Display(Name = "Kích thước")]
    [Required(ErrorMessage = "Trường này không được bỏ trống")]
    [MaxLength(50)]
    public string? TenKichThuoc { get; set; }

    public virtual ICollection<ChiTietHang> ChiTietHangs { get; set; } = new List<ChiTietHang>();
}
