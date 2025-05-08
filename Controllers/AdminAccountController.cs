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
using System.Security.Claims;
using System.Security.Cryptography;

namespace BTL_TKWeb.Controllers
{
    [Authorize]
    public class AdminAccountController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get;}
        public AdminAccountController(QuanLyChanGaGoiDemContext cntxt,INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult TaoTaiKhoanAdmin()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ViewData["ChucVu"] = new SelectList(Context.CongViecs, "MaCv", "TenCv");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TaoTaiKhoanAdmin(AdminRegisterViewModel model)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ViewData["ChucVu"] = new SelectList(Context.CongViecs, "MaCv", "TenCv");
            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidateMaNV(model.MaNv.Trim()) || ValidatePhone(model.DienThoai.Trim()))
                    {
                        NotyfService.Error("Số điện thoại hoặc mã nhân viên đã tồn tại");
                        return View(model);
                    }
                    string salt = Tool.RandomString(6);
                    NhanVien nv = new NhanVien
                    {
                        MaNv = model.MaNv.Trim(),
                        TenNv = model.TenNv.Trim(),
                        Ngaysinh = model.Ngaysinh,
                        GioiTinh = model.GioiTinh,
                        DienThoai = model.DienThoai.Trim(),
                        MaCv = model.MaCv,
                        MatKhau = (model.MatKhau + salt).ToMD5(),
                        NghiViec = false,
                        Salt = salt
                    };
                    try
                    {
                        Context.Add(nv);
                        await Context.SaveChangesAsync();
                        NotyfService.Success("Tạo tài khoản thành công");
                        /*//Luu session MaNV
                        HttpContext.Session.SetString("MaNV", nv.MaNv);
                        var taikhoanID = HttpContext.Session.GetString("MaNV");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,nv.TenNv),
                            new Claim("MaNV",nv.MaNv)
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);*/
                        return RedirectToAction("Index", "AdminQuanLyNhanSu");
                    }
                    catch
                    {
                        return RedirectToAction("TaoTaiKhoanAdmin", "AdminAccount");
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
        public bool ValidateMaNV(string MaNV)
        {
            return Context.NhanViens.Any(x => x.MaNv == MaNV);
        }
        public bool ValidatePhone(string DienThoai)
        {
            return Context.NhanViens.Any(x => x.DienThoai == DienThoai);
        }
        [AllowAnonymous]
        public IActionResult DangNhapAdmin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if(taikhoanID != null) return RedirectToAction("Index", "Admin");
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public  async Task<IActionResult> DangNhapAdmin(LoginAdminViewModel model,string returnUrl = null)
        {
            try
            {
            if (ModelState.IsValid)
                {
                    var nv = Context.NhanViens.AsNoTracking().SingleOrDefault(x => x.MaNv == model.UserName);
                    if(nv == null)
                    {
                        NotyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(model);
                    }
                    if (nv.MatKhau != (model.Password + nv.Salt.Trim()).ToMD5() && (bool)!nv.NghiViec)
                    {
                        NotyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(model);
                    }
                    //Luu session MaNV
                    HttpContext.Session.SetString("MaNV", nv.MaNv);
                    var taikhoanID = HttpContext.Session.GetString("MaNV");
                    //Identity
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,nv.TenNv),
                            new Claim("MaNV",nv.MaNv)
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch(Exception ex)
            {
                NotyfService.Error("Có lỗi xảy ra");
                return View(model);
            }
            return View(model);
        }
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaNV");
            return RedirectToAction("Index", "Admin");
        }
    }
}
