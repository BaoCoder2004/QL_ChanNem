using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        private int perpage = 6;
        public ShopController(QuanLyChanGaGoiDemContext cntxt, INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        public IActionResult Index(string? search,string? id,int? sort,int? page)
        {
            var dmhang = Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                .OrderBy(x => x.ChiTietHangs.First().DonGiaBan).ToList();
            if (sort != null && sort == 2)
            {
                var danhmuc = from dm in Context.DanhMucHangs
                              join cth in Context.ChiTietHangs
                              on dm.MaHang equals cth.MaHang
                              join ctd in Context.ChiTietDdhs
                              on cth.MaChiTietHang equals ctd.MaChiTietHang
                              group ctd by dm.MaHang into g
                              select new { mh = g.Key, slb = g.Sum(g => g.SoLuong) };
                var temp = danhmuc.ToList();
                List<DanhMucHang> hang = new List<DanhMucHang>();
                foreach (var mh in temp)
                {
                    hang.Add(Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                        .Single(x => x.MaHang == mh.mh));
                }
                hang.AddRange(Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                    .Where(x => !hang.Contains(x)).ToList());

                dmhang = hang;
                ViewBag.sort=sort;
            }
            dmhang=dmhang.Where(x=>!x.NgungKinhDoanh).ToList();
            foreach (DanhMucHang c in dmhang)
            {
                c.ChiTietHangs.First().AnhHangs = Context.AnhHangs
                    .Where(x => x.MaChiTietHang == c.ChiTietHangs.First().MaChiTietHang).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                dmhang=dmhang.Where(x=>x.TenHang.Contains(search)).ToList();
                ViewBag.search=search;
            }
            if (!string.IsNullOrEmpty(id)&&id!="ALL")
            {
                dmhang=dmhang.Where(x=>x.MaLoai==id).ToList();
                ViewBag.id=id;
            }
            ViewBag.pagenum = (int)Math.Ceiling(dmhang.Count() / (double)perpage);
            if (page != null &&page> 0) ViewBag.current = page;
            else
            {
                page = 1;
                ViewBag.current = page;
            }
            dmhang=dmhang.Skip(perpage*((int)page-1)).Take(perpage).ToList();
            return View(dmhang);
        }
        public IActionResult IndexContent(string? search, string? id, int? sort, int? page)
        {
            var dmhang = Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                .OrderBy(x => x.ChiTietHangs.First().DonGiaBan).ToList();
            if (sort != null && sort == 2)
            {
                var danhmuc = from dm in Context.DanhMucHangs
                              join cth in Context.ChiTietHangs
                              on dm.MaHang equals cth.MaHang
                              join ctd in Context.ChiTietDdhs
                              on cth.MaChiTietHang equals ctd.MaChiTietHang
                              group ctd by dm.MaHang into g
                              select new { mh = g.Key, slb = g.Sum(g => g.SoLuong) };
                var temp = danhmuc.ToList();
                List<DanhMucHang> hang = new List<DanhMucHang>();
                foreach (var mh in temp)
                {
                    hang.Add(Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                        .Single(x => x.MaHang == mh.mh));
                }
                hang.AddRange(Context.DanhMucHangs.Include(x => x.ChiTietHangs)
                    .Where(x => !hang.Contains(x)).ToList());

                dmhang = hang;
                ViewBag.sort = sort;
            }
            dmhang = dmhang.Where(x => !x.NgungKinhDoanh).ToList();
            foreach (DanhMucHang c in dmhang)
            {
                c.ChiTietHangs.First().AnhHangs = Context.AnhHangs
                    .Where(x => x.MaChiTietHang == c.ChiTietHangs.First().MaChiTietHang).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                dmhang = dmhang.Where(x => x.TenHang.Contains(search)).ToList();
                ViewBag.search = search;
            }
            if (!string.IsNullOrEmpty(id)&&id!="ALL")
            {
                dmhang = dmhang.Where(x => x.MaLoai == id).ToList();
                ViewBag.id = id;
            }
            ViewBag.pagenum = (int)Math.Ceiling(dmhang.Count() / (double)perpage);
            if (page != null && page > 0) ViewBag.current = page;
            else
            {
                page = 1;
                ViewBag.current = page;
            }
            dmhang = dmhang.Skip(perpage * ((int)page - 1)).Take(perpage).ToList();
            return PartialView("ShopTable",dmhang);
        }
        public IActionResult Search(string? search)
        {
            if (!string.IsNullOrEmpty(search)) return RedirectToAction("Index",new RouteValueDictionary(new { controller = "Shop", action = "Index", search = search }));
            return Redirect(Request.Headers["Referer"].ToString());

        }
    }
}
