using PagedList;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        [ChildActionOnly]
        public ActionResult SanPhamPartial1()
        {
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            return PartialView(lstSP);
        }
        public ActionResult SanPhamPartial2()
        {
            return PartialView();
        }
        //Xem chi tiet
        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        // Load SP 
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {

            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            // phân trang
            int Pagesize = 3;
            int Pagenum = (page ?? 1);
            ViewBag.MaLoai = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(Pagenum, Pagesize));
        }
    }
}