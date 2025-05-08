using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_TKWeb.Models;

namespace BTL_TKWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext _context;

        public KhachHangController(QuanLyChanGaGoiDemContext context)
        {
            _context = context;
        }

        // GET: KhachHang
        public async Task<IActionResult> Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            return View(await _context.KhachHangs.Include(x=>x.DonDatHangs).ToListAsync());
        }

        // GET: KhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index",new RouteValueDictionary ( new {controller="DonDatHang",action="Index",search=khachHang.MaKh}));
        }
    }
}
