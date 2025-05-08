using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.ViewComponents
{
    public class RenderThemSPViewComponent:ViewComponent
    {
        private QuanLyChanGaGoiDemContext Context;
        public RenderThemSPViewComponent(QuanLyChanGaGoiDemContext context)
        {
            Context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string MaHang)
        {
            List<DanhMucHang> danhMucHangs=Context.DanhMucHangs.Include(x=>x.ChiTietHangs)
                .Where(x=>x.MaHang!=MaHang).Take(5).ToList();
            foreach(DanhMucHang d in danhMucHangs)
            {
                d.ChiTietHangs.First().AnhHangs.Add(Context.AnhHangs
                    .FirstOrDefault(x => x.MaChiTietHang == d.ChiTietHangs.First().MaChiTietHang));
            }
            return View("RenderThemSP",danhMucHangs);
        }
    }
}
