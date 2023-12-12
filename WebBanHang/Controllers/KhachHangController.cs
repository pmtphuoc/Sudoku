using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: KhachHang
        public ActionResult Index()
        {
            //C1 var lstKH = from kh in db.KhachHangs select kh;
            var lstKH = db.KhachHangs.ToList();
            return View(lstKH);
        }
        public ActionResult MotDoiTuong()
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);

            return View(kh);
        }
        public ActionResult Sort()
        {
            List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKH);
        }
        public ActionResult Group()
        {
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}