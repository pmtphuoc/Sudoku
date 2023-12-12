using System.Linq;

namespace WebBanHang.Models
{
    public class GioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        public GioHang(int maSP)
        {
            using (BanHangDataContext db = new BanHangDataContext())
            {
                this.MaSP = maSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == maSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public GioHang(int maSP, int sl)
        {
            using (BanHangDataContext db = new BanHangDataContext())
            {
                this.MaSP = maSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == maSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public GioHang() { }
    }
}