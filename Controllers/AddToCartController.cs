using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Extension;
using BTL_TKWeb.Models;
using BTL_TKWeb.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BTL_TKWeb.Controllers
{
    public class AddToCartController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        
        public AddToCartController(QuanLyChanGaGoiDemContext cntxt, INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        public IActionResult Add(string id, int soluong)
        {
            var gioHang = HttpContext.Session.Get<List<CartItem>>("GioHang");
            
            CartItem item=new CartItem();
            if (gioHang == null)
            {
                gioHang= new List<CartItem>();
            }
            if (gioHang.Any(x => x.Hang.MaChiTietHang == id))
            {
                ChiTietHang temp = Context.ChiTietHangs.SingleOrDefault(x => x.MaChiTietHang == id);
                if (temp == null) return NotFound();
                item = gioHang.SingleOrDefault(x => x.Hang.MaChiTietHang == id);
                if (soluong <= 0 || soluong > temp.SoLuong)
                {
                    return Json(new { success = false });
                }
                item.SoLuong = soluong;
                item.Hang = temp;
            }
            else
            {
                ChiTietHang temp = Context.ChiTietHangs.SingleOrDefault(x => x.MaChiTietHang == id);
                if (temp == null) return NotFound();
                if (soluong <= 0 || soluong > temp.SoLuong)
                {
                    return Json(new { success = false });
                }
                item.SoLuong = soluong;
                item = new CartItem
                {
                    SoLuong = soluong,
                    Hang = temp
                };
                gioHang.Add(item);
            }
            HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
            return Json(new {success=true});
        }
        public IActionResult Delete(string id)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            var gioHang = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (gioHang == null||gioHang.Count==0) return Json(new { success = false });
            if (gioHang.Any(x=>x.Hang.MaChiTietHang==id))
            {
                var itemToRemove = gioHang.SingleOrDefault(x => x.Hang.MaChiTietHang == id);
                gioHang.Remove(itemToRemove);
            }
            HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
            return Json(new { success = true });
        }
        
    }
}
