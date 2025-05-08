using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_TKWeb.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.ModelView;
using BTL_TKWeb.Helper;

namespace BTL_TKWeb.Controllers
{
    public class DanhMucHangController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext _context;
        public INotyfService NotyfService;

        public DanhMucHangController(QuanLyChanGaGoiDemContext context,INotyfService notyf)
        {
            _context = context;
            NotyfService = notyf;
        }

        // GET: DanhMucHang
        public async Task<IActionResult> Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            var quanLyChanGaGoiDemContext = _context.DanhMucHangs.Include(d => d.MaLoaiNavigation)
                .Include(x=>x.ChiTietHangs).Where(x => x.ChiTietHangs.Count == 0).ToList();
            quanLyChanGaGoiDemContext.AddRange(_context.DanhMucHangs.Include(d => d.MaLoaiNavigation)
                .Where(x => !quanLyChanGaGoiDemContext.Contains(x)).Include(x => x.ChiTietHangs)
                .OrderByDescending(x => x.ChiTietHangs.Count(x => x.SoLuong == 0)));
            return View(quanLyChanGaGoiDemContext.ToList());
        }

        // GET: DanhMucHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id == null)
            {
                return NotFound();
            }

            var danhMucHang = _context.DanhMucHangs
                .Include(d => d.MaLoaiNavigation)
                .FirstOrDefault(m => m.MaHang == id);
            if (danhMucHang == null)
            {
                return NotFound();
            }
            danhMucHang.ChiTietHangs=_context.ChiTietHangs.Where(x => x.MaHang == id)
                .Include(x=>x.MaChatLieuNavigation).Include(x=> x.MaKichThuocNavigation)
                .Include(x=>x.MaMauNavigation).Include(x=>x.AnhHangs).ToList();
            return View(danhMucHang);
        }

        // GET: DanhMucHang/Create
        public IActionResult Create()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ViewData["MaLoai"] = new SelectList(_context.TheLoais, "MaLoai", "TenLoai");
            return View();
        }

        // POST: DanhMucHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHang,TenHang,MaLoai,ThongTin,ThoiGianBaoHanh,NgungKinhDoanh")] DanhMucHang danhMucHang)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ViewData["MaLoai"] = new SelectList(_context.TheLoais, "MaLoai", "TenLoai", danhMucHang.MaLoai);
            if (ModelState.IsValid)
            {
                if (_context.DanhMucHangs.Any(x => x.MaHang == danhMucHang.MaHang.Trim()))
                {
                    NotyfService.Error("Đã tồn tại mã sản phẩm này");
                    return View(danhMucHang);
                }
                _context.Add(danhMucHang);
                await _context.SaveChangesAsync();
                NotyfService.Success("Thêm thành công, hãy thêm các thông số và ảnh sản phẩm trong mục thông tin sản phẩm");
                return RedirectToAction(nameof(Index));
            }
            return View(danhMucHang);
        }

        // GET: DanhMucHang/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id == null)
            {
                return NotFound();
            }

            var danhMucHang = await _context.DanhMucHangs.FindAsync(id);
            if (danhMucHang == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.TheLoais, "MaLoai", "TenLoai", danhMucHang.MaLoai);
            return View(danhMucHang);
        }
        public async Task<IActionResult> EditCTH(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id == null)
            {
                return NotFound();
            }
            ChiTietHang chiTietHang= _context.ChiTietHangs.Find(id);
            chiTietHang.MaHangNavigation= _context.DanhMucHangs.Single(x=>x.MaHang==chiTietHang.MaHang);
            chiTietHang.AnhHangs.Add(_context.AnhHangs.Single(x=>x.MaChiTietHang==chiTietHang.MaChiTietHang));
            if (chiTietHang == null)
            {
                return NotFound();
            }
            ChiTietHangVaAnh chiTietHangVaAnh = new ChiTietHangVaAnh
            {
                MaChiTietHang = chiTietHang.MaChiTietHang,
                MaHang = chiTietHang.MaHang,
                MaChatLieu = chiTietHang.MaChatLieu,
                MaKichThuoc = chiTietHang.MaKichThuoc,
                MaMau = chiTietHang.MaMau,
                SoLuong = chiTietHang.SoLuong,
                DonGiaNhap = chiTietHang.DonGiaNhap,
                DonGiaBan = chiTietHang.DonGiaBan,
                MaHangNavigation=chiTietHang.MaHangNavigation,
                AnhHangs=chiTietHang.AnhHangs
            };
            ViewData["MaMau"] = new SelectList(_context.MauSacs, "MaMau", "TenMau", chiTietHang.MaMau);
            ViewData["MaKichThuoc"]=new SelectList(_context.KichThuocs,"MaKichThuoc","TenKichThuoc",chiTietHang.MaKichThuoc);
            ViewData["MaChatLieu"] = new SelectList(_context.ChatLieus, "MaChatLieu", "TenChatLieu", chiTietHang.MaChatLieu);
            return View(chiTietHangVaAnh);
        }
        // POST: DanhMucHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DanhMucHang danhMucHang)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id != danhMucHang.MaHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucHangExists(danhMucHang.MaHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.TheLoais, "MaLoai", "MaLoai", danhMucHang.MaLoai);
            return View(danhMucHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCTH(string id, ChiTietHangVaAnh chiTietHangVaAnh)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id != chiTietHangVaAnh.MaChiTietHang)
            {
                return NotFound();
            }
            ChiTietHang chiTietHang = new ChiTietHang
            {
                MaChiTietHang = chiTietHangVaAnh.MaChiTietHang,
                MaHang = chiTietHangVaAnh.MaHang,
                MaChatLieu = chiTietHangVaAnh.MaChatLieu,
                MaKichThuoc = chiTietHangVaAnh.MaKichThuoc,
                MaMau = chiTietHangVaAnh.MaMau,
                SoLuong = chiTietHangVaAnh.SoLuong,
                DonGiaNhap = chiTietHangVaAnh.DonGiaNhap,
                DonGiaBan = chiTietHangVaAnh.DonGiaBan
            };

            if (ModelState.IsValid)
            {
                if (chiTietHangVaAnh.Anh != null)
                {
                    string filename = chiTietHangVaAnh.MaChiTietHang+DateTime.Now.Ticks.ToString("") +
                        Path.GetExtension(Path.GetFileName(chiTietHangVaAnh.Anh.FileName));
                    Tool.UploadAnh(chiTietHangVaAnh.Anh, filename);
                    AnhHang anh=new AnhHang
                    {
                        TenAnh=filename,
                        MaChiTietHang=chiTietHangVaAnh.MaChiTietHang
                    };
                    _context.Remove(_context.AnhHangs.Single(x=>x.MaChiTietHang==chiTietHangVaAnh.MaChiTietHang));
                    _context.AnhHangs.Add(anh);
                    Tool.DeleteAnh(_context.AnhHangs.Single(x => x.MaChiTietHang == chiTietHangVaAnh.MaChiTietHang).TenAnh);
                }

                try
                {
                    _context.Update(chiTietHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucHangExists(chiTietHang.MaChiTietHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            chiTietHangVaAnh.MaHangNavigation = _context.DanhMucHangs.Single(x => x.MaHang == chiTietHangVaAnh.MaHang);
            chiTietHangVaAnh.AnhHangs.Add( _context.AnhHangs.Single(x=>x.MaChiTietHang==chiTietHangVaAnh.MaChiTietHang));
            ViewData["MaMau"] = new SelectList(_context.MauSacs, "MaMau", "TenMau", chiTietHang.MaMau);
            ViewData["MaKichThuoc"] = new SelectList(_context.KichThuocs, "MaKichThuoc", "TenKichThuoc", chiTietHang.MaKichThuoc);
            ViewData["MaChatLieu"] = new SelectList(_context.ChatLieus, "MaChatLieu", "TenChatLieu", chiTietHang.MaChatLieu);
            return View(chiTietHangVaAnh);
        }

        // GET: DanhMucHang/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id == null)
            {
                return NotFound();
            }

            var danhMucHang = await _context.DanhMucHangs
                .Include(d => d.MaLoaiNavigation).Include(d => d.ChiTietHangs)
                .FirstOrDefaultAsync(m => m.MaHang == id);
            if (danhMucHang == null)
            {
                return NotFound();
            }
            foreach(ChiTietHang d in danhMucHang.ChiTietHangs)
            {
                if (_context.ChiTietDdhs.Any(x => x.MaChiTietHang == d.MaChiTietHang)||
                    _context.ChiTietHdns.Any(x => x.MaChiTietHang == d.MaChiTietHang))
                {
                    NotyfService.Error("Không thể xoá sảm phẩm do sản phẩm đã có trong đơn hàng");
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(danhMucHang);
        }

        // POST: DanhMucHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            DanhMucHang danhMucHang =  _context.DanhMucHangs.Find(id);
            if (danhMucHang == null) return NotFound();
            else
            {
                List<ChiTietHang> cth = _context.ChiTietHangs.Include(x => x.AnhHangs).Where(x => x.MaHang == danhMucHang.MaHang).ToList();
                if(cth!=null&&cth.Count>0) 
                foreach (ChiTietHang d in cth)
                {
                    _context.AnhHangs.Remove(d.AnhHangs.First());
                    _context.SaveChanges();
                    _context.ChiTietHangs.Remove(d);
                    _context.SaveChanges();
                }
                _context.DanhMucHangs.Remove(danhMucHang);
            }
            _context.SaveChanges();
            NotyfService.Success("Xoá SP thành công");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateCTH(string MaHang)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ChiTietHangVaAnh temp = new ChiTietHangVaAnh
            {
                MaHang = MaHang,
                MaChiTietHang = "none",
                MaHangNavigation = _context.DanhMucHangs.Single(x => x.MaHang == MaHang)
            };
            ViewData["MaMau"] = new SelectList(_context.MauSacs, "MaMau", "TenMau");
            ViewData["MaKichThuoc"] = new SelectList(_context.KichThuocs, "MaKichThuoc", "TenKichThuoc");
            ViewData["MaChatLieu"] = new SelectList(_context.ChatLieus, "MaChatLieu", "TenChatLieu");
            return View(temp);
        }

        // POST: DanhMucHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCTH(ChiTietHangVaAnh chiTietHangVaAnh)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            ViewData["MaMau"] = new SelectList(_context.MauSacs, "MaMau", "TenMau", chiTietHangVaAnh.MaMau);
            ViewData["MaKichThuoc"] = new SelectList(_context.KichThuocs, "MaKichThuoc", "TenKichThuoc", chiTietHangVaAnh.MaKichThuoc);
            ViewData["MaChatLieu"] = new SelectList(_context.ChatLieus, "MaChatLieu", "TenChatLieu", chiTietHangVaAnh.MaChatLieu);
            if (ModelState.IsValid)
            {
                if (chiTietHangVaAnh.Anh != null)
                {
                    ChiTietHang chiTietHang = new ChiTietHang
                    {
                        MaHang = chiTietHangVaAnh.MaHang,
                        MaChatLieu = chiTietHangVaAnh.MaChatLieu,
                        MaKichThuoc = chiTietHangVaAnh.MaKichThuoc,
                        MaMau = chiTietHangVaAnh.MaMau,
                        SoLuong = chiTietHangVaAnh.SoLuong,
                        DonGiaNhap = chiTietHangVaAnh.DonGiaNhap,
                        DonGiaBan = chiTietHangVaAnh.DonGiaBan
                    };
                    for (int i = 1; ; i++)
                    {
                        string temp = chiTietHang.MaHang + "-" + i;
                        if (!_context.ChiTietHangs.Any(x => x.MaChiTietHang == temp))
                        {
                            chiTietHang.MaChiTietHang = temp;
                            break;
                        }
                    }
                    _context.Add(chiTietHang);
                    _context.SaveChanges();
                    string filename = chiTietHang.MaChiTietHang + DateTime.Now.Ticks.ToString("") +
                        Path.GetExtension(chiTietHangVaAnh.Anh.FileName);
                    Tool.UploadAnh(chiTietHangVaAnh.Anh, filename);
                    AnhHang anh = new AnhHang
                    {
                        TenAnh = filename,
                        MaChiTietHang = chiTietHang.MaChiTietHang
                    };
                    _context.AnhHangs.Add(anh);
                    await _context.SaveChangesAsync();
                    NotyfService.Success("Thêm chi tiết thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    NotyfService.Error("Hãy chọn một ảnh");
                    return View(chiTietHangVaAnh);
                }
            }
            return View(chiTietHangVaAnh);
        }
        public async Task<IActionResult> DeleteCTH(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHang=_context.ChiTietHangs.Include(x=>x.MaChatLieuNavigation)
                .Include(x=>x.MaMauNavigation).Include(x=>x.MaKichThuocNavigation)
                .Include(x=>x.MaHangNavigation)
                .FirstOrDefault(x=>x.MaChiTietHang==id);
            if (chiTietHang == null)
            {
                return NotFound();
            }
            if (_context.ChiTietDdhs.Any(x => x.MaChiTietHang == chiTietHang.MaChiTietHang) ||
                    _context.ChiTietHdns.Any(x => x.MaChiTietHang == chiTietHang.MaChiTietHang))
            {
                NotyfService.Error("Không thể xoá sảm phẩm do sản phẩm đã có trong đơn hàng");
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietHang);
        }

        // POST: DanhMucHang/Delete/5
        
        public async Task<IActionResult> DeleteCTHConfirmed(string id)
        {

            ChiTietHang chiTietHang = _context.ChiTietHangs.Include(x=>x.AnhHangs).Single(x=>x.MaChiTietHang==id);
            if (chiTietHang != null)
            {
                _context.AnhHangs.Remove(chiTietHang.AnhHangs.First());
                _context.SaveChanges();
                _context.ChiTietHangs.Remove(chiTietHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietHang);
        }
        public IActionResult ThemChiTiet()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            return View();
        }
        [HttpPost]
        public IActionResult ThemChiTiet(ThongSo thongSo)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            bool dathem=false;
            if (thongSo.MaChatLieu!=null&&thongSo.TenChatLieu!=null)
            {
                if (_context.ChatLieus.Any(x => x.MaChatLieu == thongSo.MaChatLieu||
                x.TenChatLieu==thongSo.TenChatLieu))
                {
                    NotyfService.Error("Đã tồn tại mã chất liệu hoặc tên chất liệu");
                }
                else
                {
                    ChatLieu temp = new ChatLieu
                    {
                        MaChatLieu = thongSo.MaChatLieu,
                        TenChatLieu = thongSo.TenChatLieu
                    };
                    _context.ChatLieus.Add(temp);
                    _context.SaveChanges();
                    NotyfService.Success("Thêm chất liệu thành công");
                    dathem = true;
                }
            }
            if (thongSo.MaMau != null && thongSo.TenMau != null)
            {
                if (_context.MauSacs.Any(x => x.MaMau == thongSo.MaMau||x.TenMau==thongSo.TenMau))
                {
                    NotyfService.Error("Đã tồn tại mã màu hoặc tên màu");
                }
                else
                {
                    MauSac temp = new MauSac { MaMau = thongSo.MaMau, TenMau = thongSo.TenMau };
                    _context.MauSacs.Add(temp);
                    _context.SaveChanges();
                    NotyfService.Success("Thêm màu sắc thành công");
                    dathem = true;
                }
            }
            if (thongSo.MaKichThuoc != null && thongSo.TenKichThuoc != null)
            {
                if (_context.KichThuocs.Any(x => x.MaKichThuoc == thongSo.MaKichThuoc ||
                x.TenKichThuoc == thongSo.TenKichThuoc))
                {
                    NotyfService.Error("Đã tồn tại mã kích thước hoặc tên kích thước");
                }
                else
                {
                    KichThuoc temp = new KichThuoc { MaKichThuoc = thongSo.MaKichThuoc, 
                    TenKichThuoc = thongSo.TenKichThuoc };
                _context.KichThuocs.Add(temp);
                _context.SaveChanges();
                NotyfService.Success("Thêm kích thước thành công");
                dathem = true;
                }
            }
            if(dathem) return View();
            NotyfService.Error("Hãy nhập đầy đủ mã và tên cho ít nhất 1 loại");
            return View();
        }
        [HttpPost]
        public IActionResult XoaChiTiet(ThongSo thongSo)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            bool dathem = false;
            if (thongSo.MaKichThuoc != null)
            {
                if (!_context.KichThuocs.Any(x => x.MaKichThuoc == thongSo.MaKichThuoc))
                {
                    NotyfService.Error("Không tồn tại mã kích thước");
                }
                else
                 {   _context.KichThuocs.Remove(_context.KichThuocs.Single(x=>x.MaKichThuoc==thongSo.MaKichThuoc));
                _context.SaveChanges();
                NotyfService.Success("Xoá kích thước thành công");
                    dathem = true;
                }
            }
            if(thongSo.MaChatLieu != null)
            {
                if (!_context.ChatLieus.Any(x => x.MaChatLieu == thongSo.MaChatLieu))
                {
                    NotyfService.Error("Không tồn tại mã chất liệu");
                }
                else
                {    _context.ChatLieus.Remove(_context.ChatLieus.Single(x=>x.MaChatLieu==thongSo.MaChatLieu));
                _context.SaveChanges();
                NotyfService.Success("Xoá chất liêu thành công");
                    dathem = true;
                }
            }
            if(thongSo.MaMau!=null)
            {
                if (!_context.MauSacs.Any(x => x.MaMau == thongSo.MaMau))
                {
                    NotyfService.Error("Không tồn tại mã màu");
                }
                else
                {    _context.MauSacs.Remove(_context.MauSacs.Single(x=>x.MaMau==thongSo.MaMau));
                _context.SaveChanges();
                NotyfService.Success("Xoá màu sắc thành công");
                    dathem = true;
                }
            }
            if(dathem) return View("ThemChiTiet");
            NotyfService.Error("Hãy chọn một mã để xoá");
            return View("ThemChiTiet");
        }
        private bool DanhMucHangExists(string id)
        {
            return _context.DanhMucHangs.Any(e => e.MaHang == id);
        }
    }
}
