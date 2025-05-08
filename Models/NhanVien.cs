using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class NhanVien
{
    [MaxLength(20)]
    [Required(ErrorMessage = "Vui lòng nhập mã nhân viên")]
    [Display(Name = "Mã nhân viên")]
    public string MaNv { get; set; } = null!;
    [MaxLength(60)]
    [Required(ErrorMessage = "Vui lòng nhập tên nhân viên")]
    [Display(Name = "Tên nhân viên")]
    public string? TenNv { get; set; }
    [Range(typeof(DateOnly), "1/1/1945", "31/12/2007", ErrorMessage = "Giá trị ngày phải thuộc khoảng từ 1/1/1945 đến 31/12/2007")]
    [DataType(DataType.Date, ErrorMessage = "Ngày sinh bắt buộc phải được nhập")]
    [Display(Name = "Ngày sinh")]
    [Required(ErrorMessage = "Ngày sinh bắt buộc phải được nhập")]
    public DateOnly? Ngaysinh { get; set; }
    [Display(Name = "Giới tính")]
    public bool GioiTinh { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [MaxLength(10)]
    [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
    [Display(Name = "Điện thoại")]
    public string? DienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? MaCv { get; set; }
    [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu có độ dài từ 6 - 32 ký tự")]
    [RegularExpression(@"^(?=.*\w+)(?=.*[A-Z]+)(?=.*\d+)(?=.*[!#$%&/()=?»«@|£§€{}*.;'<>_,-]+).*$", ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự số, ký tự đặc biệt, chữ cái viết hoa và chữ cái thường")]
    [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
    [Display(Name = "Mật khẩu")]
    public string? MatKhau { get; set; }
    [Display(Name = "Trạng thái")]
    public bool NghiViec { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();

    public virtual CongViec? MaCvNavigation { get; set; }
}
