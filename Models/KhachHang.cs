using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class KhachHang
{
    [Display(Name = "Mã khách hàng")]
    public string MaKh { get; set; } = null!;
    [Display(Name = "Tên khách hàng")]
    public string? TenKhach { get; set; }
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }
    [Display(Name = "Điện thoại")]
    public string? DienThoai { get; set; }
    [Display(Name = "Mật khẩu")]
    public string? MatKhau { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();
}
