using BTL_TKWeb.Models;

namespace BTL_TKWeb.ModelView
{
    public class ThongTinGioHang
    {
        public List<CartItem> Hangs {  get; set; }
        public KhachHang KhachHang {  get; set; }
        public bool LanDau;
        public decimal TinhTien()
        {
            decimal temp = 0;
            temp=Hangs.Sum(x=>x.ThanhTien);
            return temp;
        }
    }
}
