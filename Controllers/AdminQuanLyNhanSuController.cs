using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Extension;
using BTL_TKWeb.Helper;
using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.Controllers
{
    public class AdminQuanLyNhanSuController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        public AdminQuanLyNhanSuController(QuanLyChanGaGoiDemContext cntxt, INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if(Context.NhanViens.Single(x=>x.MaNv==taikhoanID).MaCv!="QUANLY") return NotFound();
            List<NhanVien> dsnv=Context.NhanViens.Where(x=>x.MaNv!="ONLINE").Include(x => x.MaCvNavigation).ToList();
            return View(dsnv);
        }
        public IActionResult TaoTaiKhoan()
        {
            return RedirectToAction("TaoTaiKhoanAdmin", "AdminAccount");
        }
        public async Task<IActionResult> Detail(string? MaNV)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (taikhoanID != MaNV &&
                Context.NhanViens.Single(x => x.MaNv == taikhoanID).MaCv != "QUANLY") return NotFound();
            if (MaNV==null) return NotFound();
            var temp=await Context.NhanViens.Include(x=>x.MaCvNavigation).FirstOrDefaultAsync(y=>y.MaNv==MaNV);
            if (temp == null) return NotFound();
            return View(temp);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? MaNV)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (taikhoanID != MaNV &&
                Context.NhanViens.Single(x => x.MaNv == taikhoanID).MaCv != "QUANLY") return NotFound();
            if (MaNV == null) return NotFound();
            var temp = await Context.NhanViens.Include(x => x.MaCvNavigation).FirstOrDefaultAsync(y => y.MaNv == MaNV);
            if (temp == null) return NotFound();
            NhanVien nhanVien=new NhanVien
            {
                MaNv = temp.MaNv,
                MaCv = temp.MaCv,
                TenNv = temp.TenNv,
                DienThoai=temp.DienThoai,
                GioiTinh=temp.GioiTinh,
                Ngaysinh=temp.Ngaysinh
            };
            ViewData["ChucVu"] = new SelectList(Context.CongViecs, "MaCv", "TenCv",nhanVien.MaCv);
            return View(nhanVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string MaNV,NhanVien nhanVien)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (taikhoanID != MaNV &&
                Context.NhanViens.Single(x => x.MaNv == taikhoanID).MaCv != "QUANLY") return NotFound();
            if (MaNV !=nhanVien.MaNv) return NotFound();
            if (ModelState.IsValid)
            {
                if (Context.NhanViens.Any(x => x.MaNv != nhanVien.MaNv && x.DienThoai==nhanVien.DienThoai))
                {
                    NotyfService.Error("Đã tồn tại SĐT này");
                    return View(nhanVien);
                }
                try
                {
                    string salt = Tool.RandomString(6);
                    NhanVien temp = new NhanVien
                    {
                        MaNv = nhanVien.MaNv,
                        TenNv = nhanVien.TenNv,
                        Ngaysinh=nhanVien.Ngaysinh,
                        GioiTinh=nhanVien.GioiTinh,
                        DienThoai=nhanVien.DienThoai,
                        MaCv=nhanVien.MaCv,
                        NghiViec=nhanVien.NghiViec,
                        Salt=salt,
                        MatKhau=(nhanVien.MatKhau+salt).ToMD5(),
                    };
                    Context.Update(temp);
                    await Context.SaveChangesAsync();
                    NotyfService.Success("Lưu thành công");
                }catch (DbUpdateConcurrencyException)
                {
                    if (!Context.NhanViens.Any(x => x.MaNv == nhanVien.MaNv))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ChucVu"] = new SelectList(Context.CongViecs, "MaCv", "TenCv",nhanVien.MaCv);
            return View(nhanVien);
        }
    }
}
