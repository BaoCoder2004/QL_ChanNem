using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class MauSac
{
    [Display(Name = "Mã màu")]
    [Required(ErrorMessage = "Trường này không được bỏ trống")]
    [MaxLength(20)]
    public string MaMau { get; set; } = null!;
    [Display(Name = "Màu sắc")]
    [Required(ErrorMessage = "Trường này không được bỏ trống")]
    [MaxLength(50)]
    public string? TenMau { get; set; }

    public virtual ICollection<ChiTietHang> ChiTietHangs { get; set; } = new List<ChiTietHang>();
}
