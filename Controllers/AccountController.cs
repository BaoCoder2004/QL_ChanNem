using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Extension;
using BTL_TKWeb.Helper;
using BTL_TKWeb.Models;
using BTL_TKWeb.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace BTL_TKWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        public AccountController(QuanLyChanGaGoiDemContext context,INotyfService notyf)
        {
            Context = context;
            NotyfService = notyf;
        }
        public IActionResult DangNhap()
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID != null) return RedirectToAction("Index", "Home");
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginKHViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var nv = Context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKh == model.UserName);
                    if (nv == null)
                    {
                        NotyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(model);
                    }
                    //Luu session MaNV
                    HttpContext.Session.SetString("MaKH", nv.MaKh);
                    var taikhoanID = HttpContext.Session.GetString("MaKH");
                    //Identity
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,nv.TenKhach),
                            new Claim("MaKH",nv.MaKh)
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    NotyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                NotyfService.Error("Có lỗi xảy ra");
                return View(model);
            }
            return View(model);
        }
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaKH");
            return RedirectToAction("Index", "Home");
        }
        public bool ValidateMaNV(string MaNV)
        {
            return Context.KhachHangs.Any(x => x.MaKh == MaNV);
        }
        public bool ValidatePhone(string DienThoai)
        {
            return Context.KhachHangs.Any(x => x.DienThoai == DienThoai);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult TaoTaiKhoan()
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID != null) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TaoTaiKhoan(RegisterViewModel model)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID != null) return RedirectToAction("Index", "Home");
            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidateMaNV(model.MaKh.Trim()) || ValidatePhone(model.DienThoai.Trim()))
                    {
                        NotyfService.Error("Số điện thoại hoặc tài khoản đã tồn tại");
                        return View(model);
                    }
                    string salt = Tool.RandomString(6);
                    KhachHang nv = new KhachHang
                    {
                        MaKh = model.MaKh.Trim(),
                        TenKhach = model.TenKh.Trim(),
                        DienThoai = model.DienThoai.Trim(),
                        DiaChi = model.DiaChi.Trim(),
                        MatKhau = (model.MatKhau + salt).ToMD5(),
                        Salt = salt
                    };
                    try
                    {
                        Context.Add(nv);
                        await Context.SaveChangesAsync();
                        NotyfService.Success("Tạo tài khoản thành công");
                        //Luu session MaNV
                        HttpContext.Session.SetString("MaKH", nv.MaKh);
                        taikhoanID = HttpContext.Session.GetString("MaKH");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,nv.TenKhach),
                            new Claim("MaKH",nv.MaKh)
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        if (Request.Headers.Referer != StringValues.IsNullOrEmpty) 
                            return Redirect(Request.Headers["Referer"].ToString());
                        else return RedirectToAction("Index","Home");
                    }
                    catch
                    {
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }
        public IActionResult ThongTinKH(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            if (taikhoanID != id) return NotFound();
            KhachHang temp=Context.KhachHangs.FirstOrDefault(x=>x.MaKh==id);
            RegisterViewModel kh = new RegisterViewModel
            {
                MaKh = temp.MaKh,
                TenKh = temp.TenKhach,
                DienThoai = temp.DienThoai,
                DiaChi = temp.DiaChi
            };
            return View(kh);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ThongTinKH(RegisterViewModel model)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("Index", "Home");
            ViewData["Reffer"] = Url.Action("Index", "Home");
            try
            {
                if (ModelState.IsValid)
                {
                    if (Context.KhachHangs.Any(x=>x.MaKh!=model.MaKh&&x.DienThoai==model.DienThoai))
                    {
                        NotyfService.Error("Số điện thoại đã tồn tại");
                        return View(model);
                    }
                    string salt = Tool.RandomString(6);
                    KhachHang nv = new KhachHang
                    {
                        MaKh = model.MaKh.Trim(),
                        TenKhach = model.TenKh.Trim(),
                        DienThoai = model.DienThoai.Trim(),
                        DiaChi = model.DiaChi.Trim(),
                        MatKhau = (model.MatKhau + salt).ToMD5(),
                        Salt = salt
                    };
                    try
                    {
                        Context.Update(nv);
                        await Context.SaveChangesAsync();
                        NotyfService.Success("Lưu thành công");
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }
        public IActionResult DonHang(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            if (taikhoanID != id) return NotFound();
            List<DonDatHang> d = Context.DonDatHangs.Include(x=>x.MaKhNavigation).Where(x => x.MaKh == id)
                .OrderBy(x => x.DaXuLy).ToList();
            return View(d);
        }
        public IActionResult Details(int id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            if(!Context.DonDatHangs.Any(x=>x.MaDdh==id)) return NotFound();
            DDHvaCTDDH d = new DDHvaCTDDH
            {
                chiTietDdh = Context.ChiTietDdhs.Include(x=>x.MaChiTietHangNavigation)
                .Include(x=>x.MaChiTietHangNavigation)
                .Where(x => x.MaDdh == id).ToList(),
                donDatHang = Context.DonDatHangs.Include(x=>x.MaKhNavigation).FirstOrDefault(x => x.MaDdh == id)
            };
            if(d.donDatHang.MaKh != taikhoanID) return NotFound();
            foreach(ChiTietDdh ct in d.chiTietDdh)
            {
                ct.MaChiTietHangNavigation.MaChatLieuNavigation = Context.ChatLieus
                    .FirstOrDefault(x => x.MaChatLieu == ct.MaChiTietHangNavigation.MaChatLieu);
                ct.MaChiTietHangNavigation.MaKichThuocNavigation = Context.KichThuocs
                    .FirstOrDefault(x => x.MaKichThuoc == ct.MaChiTietHangNavigation.MaKichThuoc);
                ct.MaChiTietHangNavigation.MaMauNavigation = Context.MauSacs
                    .FirstOrDefault(x => x.MaMau == ct.MaChiTietHangNavigation.MaMau);
                ct.MaChiTietHangNavigation.MaHangNavigation = Context.DanhMucHangs
                    .FirstOrDefault(x => x.MaHang == ct.MaChiTietHangNavigation.MaHang);
            }
            return View(d);
        }
        public IActionResult Edit(int id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            if (!Context.DonDatHangs.Any(x => x.MaDdh == id&&!x.DaXuLy)) return NotFound();
            DonDatHang d = Context.DonDatHangs.Include(x=>x.MaKhNavigation)
                .FirstOrDefault(x => x.MaDdh == id);
            if (d.MaKh != taikhoanID) return NotFound();
            List<ChiTietDdh> ct=Context.ChiTietDdhs.Where(x=>x.MaDdh==id).ToList();
            foreach(ChiTietDdh c in ct)
            {
                ChiTietHang temp=Context.ChiTietHangs.FirstOrDefault(x=>x.MaChiTietHang==c.MaChiTietHang);
                temp.SoLuong += c.SoLuong;
                Context.Remove(c);
                Context.Update(temp);
            }
            Context.SaveChanges();
            Context.Remove(d);
            Context.SaveChanges();
            NotyfService.Success("Huỷ đơn hàng thành công");
            return RedirectToAction("Index", "Home");
        }
    }
}
