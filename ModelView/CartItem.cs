using BTL_TKWeb.Models;

namespace BTL_TKWeb.ModelView
{
    public class CartItem
    {
        public ChiTietHang Hang { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => SoLuong * (decimal)Hang.DonGiaBan;
    }
}
