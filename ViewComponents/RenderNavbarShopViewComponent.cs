using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTL_TKWeb.ViewComponents
{
    public class RenderNavbarShopViewComponent:ViewComponent
    {
        QuanLyChanGaGoiDemContext Context;
        public RenderNavbarShopViewComponent(QuanLyChanGaGoiDemContext context)
        {
            Context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? MaKH)
        {
            KhachHang temp=null;
            if(!string.IsNullOrEmpty(MaKH)) temp=Context.KhachHangs.SingleOrDefault(x=>x.MaKh==MaKH);
            return View("RenderNavbarShop",temp);
        }
    }
}
