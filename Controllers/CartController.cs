using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_TKWeb.Extension;
using BTL_TKWeb.Models;
using BTL_TKWeb.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly QuanLyChanGaGoiDemContext Context;
        public INotyfService NotyfService { get; }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        public CartController(QuanLyChanGaGoiDemContext cntxt, INotyfService notyfService)
        {
            Context = cntxt;
            NotyfService = notyfService;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            ThongTinGioHang gioHang = new ThongTinGioHang
            {
                Hangs = GioHang,
                KhachHang = Context.KhachHangs.SingleOrDefault(x => x.MaKh == taikhoanID)
            };
            if(gioHang.Hangs != null&& gioHang.Hangs.Count != 0)
            {
                foreach(CartItem  ct in gioHang.Hangs)
                {
                    ct.Hang.MaHangNavigation=Context.DanhMucHangs.SingleOrDefault(x=>x.MaHang==ct.Hang.MaHang);
                    ct.Hang.MaMauNavigation = Context.MauSacs.SingleOrDefault(x => x.MaMau == ct.Hang.MaMau);
                    ct.Hang.MaChatLieuNavigation = Context.ChatLieus.SingleOrDefault(x => x.MaChatLieu == ct.Hang.MaChatLieu);
                    ct.Hang.MaKichThuocNavigation = Context.KichThuocs.SingleOrDefault(x => x.MaKichThuoc == ct.Hang.MaKichThuoc);
                }
            }
            if(Context.DonDatHangs.Any(x=>x.MaKh==gioHang.KhachHang.MaKh)) gioHang.LanDau=false;
            else gioHang.LanDau = true;
            return View(gioHang);
        }
        public IActionResult ThanhToan()
        {
            var taikhoanID = HttpContext.Session.GetString("MaKH");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account");
            var gioHang = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if(gioHang == null) return NotFound();
            KhachHang khach = Context.KhachHangs.FirstOrDefault(x => x.MaKh == taikhoanID);
            DonDatHang d = new DonDatHang
            {
                MaNv = "ONLINE",
                MaKh = khach.MaKh,
                NgayMua = DateOnly.FromDateTime(DateTime.Now),
                DaXuLy = false
            };
            Context.Add(d);
            Context.SaveChanges();
            DonDatHang curr = Context.DonDatHangs.OrderByDescending(x => x.MaDdh).First();
            bool kgg = Context.DonDatHangs.Any(x => x.MaKh == khach.MaKh);
            foreach (CartItem item in gioHang)
            {
                ChiTietDdh temp = new ChiTietDdh
                {
                    MaDdh = curr.MaDdh,
                    MaChiTietHang = item.Hang.MaChiTietHang,
                    SoLuong = item.SoLuong,
                    GiamGia = kgg ? 0 : 10,
                };
                ChiTietHang tmp=Context.ChiTietHangs.FirstOrDefault(x => x.MaChiTietHang==temp.MaChiTietHang);
                tmp.SoLuong-=temp.SoLuong;
                Context.Add(temp);
                Context.Update(tmp);
                Context.SaveChanges();
            }
            using var cmd = Context.Database.GetDbConnection().CreateCommand();
            {
                cmd.CommandText = "exec TongTien @maddh=" + curr.MaDdh;
                if(cmd.Connection.State!=System.Data.ConnectionState.Open) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            HttpContext.Session.Remove("GioHang");
            NotyfService.Success("Cảm ơn bạn vì đã đặt hàng!");
            return RedirectToAction("Index", "Home");
        }
    }
}
