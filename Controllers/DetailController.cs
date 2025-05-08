using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.Controllers
{
    public class DetailController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        public DetailController(QuanLyChanGaGoiDemContext cntxt, INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        public IActionResult Index(string id)
        {
            if(string.IsNullOrEmpty(id)) return NotFound();
            DanhMucHang hang=Context.DanhMucHangs.Include(x=>x.ChiTietHangs)
                .SingleOrDefault(x=>x.MaHang==id);
            foreach(ChiTietHang ct in hang.ChiTietHangs)
            {
                ct.MaChatLieuNavigation=Context.ChatLieus.FirstOrDefault(x=>x.MaChatLieu==ct.MaChatLieu);
                ct.MaMauNavigation=Context.MauSacs.FirstOrDefault(x=>x.MaMau==ct.MaMau);
                ct.MaKichThuocNavigation=Context.KichThuocs.FirstOrDefault(x=>x.MaKichThuoc==ct.MaKichThuoc);
                ct.AnhHangs.Add(Context.AnhHangs.SingleOrDefault(x => x.MaChiTietHang == ct.MaChiTietHang));
            }
            return View(hang);
        }
    }
}
