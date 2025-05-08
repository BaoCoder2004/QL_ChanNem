using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class CongViec
{
    [Display(Name = "Mã chức vụ")]
    public string MaCv { get; set; } = null!;
    [Display(Name = "Chức vụ")]
    public string? TenCv { get; set; }

    public decimal? LuongThang { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
