using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    [Authorize(Roles = "QuanLyDH")]
    public class QuanLyDHController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: QuanLyDH
       
        public ActionResult ChuaTT()
        {
            var lst = db.DonDHs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
       
        public ActionResult ChuaGiao()
        {
            var lstcg = db.DonDHs.Where(n => n.TinhTrangGH == false&&n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            return View(lstcg);
        }
        public ActionResult DaGiaoDaTT()
        {
            var lstall = db.DonDHs.Where(n => n.TinhTrangGH == true && n.DaThanhToan == true);
            return View(lstall);
        }
        [HttpGet]
        public ActionResult DuyetDonHang (int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDH model = db.DonDHs.SingleOrDefault(n => n.MaDDH == id);
            if(model==null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.ChiTietDDHs.Where(n => n.MaDDH == id);
            ViewBag.lstChiTietDH = lstChiTietDH;
            
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDH dh)
        {
            DonDH dhup = db.DonDHs.SingleOrDefault(n => n.MaDDH == dh.MaDDH);
            dhup.DaThanhToan = dh.DaThanhToan;
            dhup.TinhTrangGH = dh.TinhTrangGH;
            db.SubmitChanges();
            var lstChiTietDH = db.ChiTietDDHs.Where(n => n.MaDDH == dh.MaDDH);
            ViewBag.lstChiTietDH = lstChiTietDH;

            return View(dhup);
        }


    }
}