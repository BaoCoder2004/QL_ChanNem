using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_TKWeb.Models;
using BTL_TKWeb.ModelView;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BTL_TKWeb.Controllers
{
    public class DonDatHangController : Controller
    {
        private int perpage = 5;
        private readonly QuanLyChanGaGoiDemContext _context;
        private INotyfService notyfService;

        public DonDatHangController(QuanLyChanGaGoiDemContext context, INotyfService notyf)
        {
            _context = context;
            notyfService = notyf;
        }

        // GET: DonDatHang
        public IActionResult Index(string? search,int? page)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            IQueryable<DonDatHang> context = _context.DonDatHangs.Include(d => d.MaKhNavigation)
                .Include(d => d.MaNvNavigation).OrderBy(x=>x.DaXuLy);
            
            if (!string.IsNullOrEmpty(search))
            {
                context=context.Where(x => x.MaKhNavigation.TenKhach.
                ToLower().Contains(search.ToLower())|| x.MaKhNavigation.MaKh.
                ToLower().Contains(search.ToLower())).OrderBy(x => x.DaXuLy);
                ViewBag.search=search;
            }
            if (page == null || page <= 0) page = 1;
            int pagenum = 0;
            if (context != null && context.Count() > 0)
            {
                pagenum = (int)Math.Ceiling(context.Count() / (float)perpage);
            }
            ViewBag.pagenum = pagenum;
            ViewBag.current = page;
            context = context.Skip(perpage * ((int)page - 1)).Take(perpage);
            return View(context.ToList());
        }
        public IActionResult IndexContent(string? search, int? page)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            IQueryable<DonDatHang> query = _context.DonDatHangs
            .Include(d => d.MaKhNavigation)
            .Include(d => d.MaNvNavigation)
            .OrderBy(x => x.DaXuLy);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.MaKhNavigation.TenKhach.ToLower().Contains(search.ToLower()))
                    .OrderBy(x => x.DaXuLy);
                ViewBag.search = search;
            }
            int pagenum = 0;
            if (query != null && query.Count() > 0)
            {
                pagenum = (int)Math.Ceiling(query.Count() / (float)perpage);
            }
            ViewBag.pagenum = pagenum;
            ViewBag.current = page;
            query = query.Skip(perpage * ((page ?? 1) - 1)).Take(perpage);
            List<DonDatHang> context = query.ToList();

            return PartialView("DonDatHangTable", context);

        }

        // GET: DonDatHang/Details/5
        public IActionResult Details(int? id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");

            if (id == null)
            {
                return NotFound();
            }
            DDHvaCTDDH dDHvaCTDDH=new DDHvaCTDDH();
            dDHvaCTDDH.donDatHang = _context.DonDatHangs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaNvNavigation).Include(x=>x.MaKhNavigation)
                .FirstOrDefault(m => m.MaDdh == id);
            if (dDHvaCTDDH.donDatHang == null)
            {
                return NotFound();
            }
            dDHvaCTDDH.chiTietDdh=_context.ChiTietDdhs.Include(x=>x.MaChiTietHangNavigation)
                .Where(x=>x.MaDdh==id).ToList();
            foreach(ChiTietDdh d in dDHvaCTDDH.chiTietDdh)
            {
                d.MaChiTietHangNavigation.MaMauNavigation=_context.MauSacs
                    .SingleOrDefault(m => m.MaMau== d.MaChiTietHangNavigation.MaMau);
                d.MaChiTietHangNavigation.MaChatLieuNavigation=_context.ChatLieus
                    .SingleOrDefault(m => m.MaChatLieu== d.MaChiTietHangNavigation.MaChatLieu);
                d.MaChiTietHangNavigation.MaKichThuocNavigation=_context.KichThuocs
                    .SingleOrDefault(m => m.MaKichThuoc== d.MaChiTietHangNavigation.MaKichThuoc);
                d.MaChiTietHangNavigation.MaHangNavigation = _context.DanhMucHangs
                    .SingleOrDefault(x => x.MaHang == d.MaChiTietHangNavigation.MaHang);
            }
            return View(dDHvaCTDDH);
        }

        
        // GET: DonDatHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");

            if (id == null)
            {
                return NotFound();
            }

            DonDatHang donDatHang = await _context.DonDatHangs.FindAsync(id);
            if (donDatHang == null)
            {
                return NotFound();
            }
            donDatHang.MaNv= HttpContext.Session.GetString("MaNV");
            donDatHang.DaXuLy=true;
            _context.Update(donDatHang);
            _context.SaveChanges();
            notyfService.Success("Cập nhật đơn hàng thành công");
            return Redirect(Request.Headers["Referer"].ToString());
        }

        
    }
}
