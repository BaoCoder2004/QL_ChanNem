using BTL_TKWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.ViewComponents
{
    public class RenderNavbarAdminViewComponent : ViewComponent
    {
        QuanLyChanGaGoiDemContext Context;
        public RenderNavbarAdminViewComponent(QuanLyChanGaGoiDemContext context)
        {
            this.Context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string MaNV)
        {
            NhanVien temp = Context.NhanViens.AsNoTracking().Single(x => x.MaNv == MaNV);
            temp.MaCvNavigation=new CongViec { TenCv=Context.CongViecs.AsNoTracking().Single(x=>x.MaCv==temp.MaCv).TenCv };
            return View("RenderNavbarAdmin",temp);
        }
    }
}
