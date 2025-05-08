using Microsoft.AspNetCore.Mvc;

namespace BTL_TKWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNV");
            if (taikhoanID == null) return RedirectToAction("DangNhapAdmin", "AdminAccount");
            return RedirectToAction("Index", "DonDatHang");
        }
    }
}
