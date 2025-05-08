using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace BTL_TKWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuanLyChanGaGoiDemContext _quanLyChanGaGoiDemContext;
        private readonly INotyfService NotyfService;

        public HomeController(ILogger<HomeController> logger,QuanLyChanGaGoiDemContext  context,INotyfService notyf)
        {
            _logger = logger;
            _quanLyChanGaGoiDemContext = context;
            NotyfService = notyf;
        }

        public IActionResult Index()
        {
            var danhmuc = from dm in _quanLyChanGaGoiDemContext.DanhMucHangs
                          join cth in _quanLyChanGaGoiDemContext.ChiTietHangs
                          on dm.MaHang equals cth.MaHang
                          join ctd in _quanLyChanGaGoiDemContext.ChiTietDdhs
                          on cth.MaChiTietHang equals ctd.MaChiTietHang
                          group ctd by dm.MaHang into g
                          select new { mh = g.Key, slb = g.Sum(g => g.SoLuong) };
            var temp=danhmuc.ToList();
            List<DanhMucHang> dmhang=new List<DanhMucHang>();
            foreach (var mh in temp)
            {
                dmhang.Add(_quanLyChanGaGoiDemContext.DanhMucHangs.Include(x=>x.ChiTietHangs)
                    .Single(x=>x.MaHang==mh.mh));
            }
            dmhang.AddRange(_quanLyChanGaGoiDemContext.DanhMucHangs.Include(x => x.ChiTietHangs)
                .Where(x=>!dmhang.Contains(x)).ToList());
            dmhang = dmhang.Where(x => !x.NgungKinhDoanh&&x.ChiTietHangs.Count>0).ToList();
            foreach (DanhMucHang c in dmhang)
            {
                c.ChiTietHangs.First().AnhHangs = _quanLyChanGaGoiDemContext.AnhHangs
                    .Where(x => x.MaChiTietHang == c.ChiTietHangs.First().MaChiTietHang).ToList();
            }
            return View(dmhang.Take(8).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
