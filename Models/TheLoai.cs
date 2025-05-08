using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class TheLoai
{
    [Display(Name = "Mã loại")]
    public string MaLoai { get; set; } = null!;
    [Display(Name = "Thể loại")]
    public string? TenLoai { get; set; }

    public virtual ICollection<DanhMucHang> DanhMucHangs { get; set; } = new List<DanhMucHang>();
}
