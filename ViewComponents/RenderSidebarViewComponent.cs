using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.ViewComponents
{
    public class RenderSidebarQuanLyViewComponent: Microsoft.AspNetCore.Mvc.ViewComponent
    {
        QuanLyChanGaGoiDemContext Context;
        public RenderSidebarQuanLyViewComponent(QuanLyChanGaGoiDemContext context)
        {
            Context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string MaNV)
        {
            NhanVien temp = Context.NhanViens.AsNoTracking().Single(x => x.MaNv == MaNV);
            temp.MaCvNavigation = new CongViec { TenCv = Context.CongViecs.AsNoTracking().Single(x => x.MaCv == temp.MaCv).TenCv };
            if (temp.MaCv != "QUANLY") ViewData["hide"] = "display: none";
            return View("RenderSidebarQuanLy");
        }
    }
}
