using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: GioHang
        public List<GioHang> LayGH()
        {
            //Gh đã có
            List<GioHang> lstGH = Session["GioHang"] as List<GioHang>;
            if (lstGH == null)
            {
                lstGH = new List<GioHang>();
                Session["GioHang"] = lstGH;

            }
            return lstGH;
        }
        
        //Add
        public ActionResult ThemGH(int MaSP, string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGH = LayGH();
            GioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            GioHang gh = new GioHang(MaSP);
            if (sp.SoLuongTon < gh.SoLuong)
            {
                return View("ThongBao");
            }
            lstGH.Add(gh);
            return Redirect(strURL);


        }
        public double TinhTongSL()
        {
            List<GioHang> lstGH = Session["GioHang"] as List<GioHang>;
            if (lstGH == null)
            {
                return 0;
            }
            return lstGH.Sum(n => n.SoLuong);
        }
        public decimal TongTien()
        {
            List<GioHang> lstGH = Session["GioHang"] as List<GioHang>;
            if (lstGH == null)
            {
                return 0;
            }
            return lstGH.Sum(n => n.ThanhTien);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSL() == 0)
            {
                ViewBag.TongSL = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XemGH()
        {
            List<GioHang> lstGH = LayGH();
            ViewBag.TongTien = TongTien();
            return View(lstGH);
        }

        [HttpGet]
        public ActionResult SuaGH(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGH = LayGH();
            GioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            //var thongtin = 
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.GioHang = lstGH;
            return View(spCheck);

        }
        [HttpPost]
        public ActionResult CapNhatGH(GioHang gh)
        {
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == gh.MaSP);
            if (spCheck.SoLuongTon < gh.SoLuong)
            {
                return View("ThongBao2");
            }
            List<GioHang> lstGH = LayGH();
            GioHang upGH = lstGH.Find(n => n.MaSP == gh.MaSP);
            upGH.SoLuong = gh.SoLuong;
            upGH.ThanhTien = upGH.SoLuong * upGH.DonGia;
            return RedirectToAction("XemGH");
        }
        public ActionResult XoaGH(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGH = LayGH();
            GioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            lstGH.Remove(spCheck);
            return RedirectToAction("XemGH");
        }
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang Khach = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //k tai khoan
                Khach = kh;
                db.KhachHangs.InsertOnSubmit(Khach);
                db.SubmitChanges();
            }
            else
            {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                Khach.TenKH = tv.HoTen;
                Khach.DiaChi = tv.DiaChi;
                Khach.Email = tv.Email;
                Khach.SoDT = tv.SoDT;
                Khach.MaTV = tv.MaLoaiTV;
                db.KhachHangs.InsertOnSubmit(Khach);
                db.SubmitChanges();
            }
            DonDH ddh = new DonDH();
            ddh.MaKH = Khach.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGH = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDHs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            //Them chi tiet dh
            List<GioHang> lst = LayGH();
            foreach (var item in lst)
            {
                ChiTietDDH ct = new ChiTietDDH();
                ct.MaDDH = ddh.MaDDH;
                ct.MaSP = item.MaSP;
                ct.TenSP = item.TenSP;
                ct.SoLuong = item.SoLuong;
                ct.DonGia = item.DonGia;
                db.ChiTietDDHs.InsertOnSubmit(ct);

            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return View("ThongBao");
        }
    }
}