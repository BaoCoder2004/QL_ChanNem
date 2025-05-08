using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL_TKWeb.Models;

public partial class ChatLieu
{
    [Display(Name = "Mã chất liệu")]
    [Required(ErrorMessage = "Trường này không được bỏ trống")]
    [MaxLength(20)]
    public string MaChatLieu { get; set; } = null!;
    [Display(Name = "Tên chất liệu")]
    [Required(ErrorMessage = "Trường này không được bỏ trống")]
    [MaxLength(50)]
    public string? TenChatLieu { get; set; }

    public virtual ICollection<ChiTietHang> ChiTietHangs { get; set; } = new List<ChiTietHang>();
}
