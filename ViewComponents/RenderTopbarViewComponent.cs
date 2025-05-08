using BTL_TKWeb.Extension;
using BTL_TKWeb.Models;
using BTL_TKWeb.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace BTL_TKWeb.ViewComponents
{
    public class RenderTopbarViewComponent:Microsoft.AspNetCore.Mvc.ViewComponent
    {
        QuanLyChanGaGoiDemContext Context;
        public RenderTopbarViewComponent(QuanLyChanGaGoiDemContext context)
        {
            Context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int c;
            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if(cartItems!=null)
            {
                c = cartItems.Count;
            }
            else
            {
                c = 0;
            }
            return View("RenderTopbar", c);
        }
    }
}
