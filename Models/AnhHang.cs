using System;
using System.Collections.Generic;

namespace BTL_TKWeb.Models;

public partial class AnhHang
{
    public string TenAnh { get; set; } = null!;

    public string MaChiTietHang { get; set; } = null!;

    public byte[]? Anh { get; set; }

    public virtual ChiTietHang MaChiTietHangNavigation { get; set; } = null!;
}
