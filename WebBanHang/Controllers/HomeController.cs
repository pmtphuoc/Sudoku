using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        public ActionResult Index()
        {
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListSP = lstSP;
            return View();
        }


        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            //Ktra captch
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "ĐĂNG KÍ THÀNH CÔNG";
                db.ThanhViens.InsertOnSubmit(tv);
                db.SubmitChanges();


                //db.ThanhViens.(tv);
                //db.SaveChanges();

                return View();
            }
            ViewBag.ThongBao = "Sai mã";
            return View();
        }
        // Dang nhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string tk = f["username"].ToString();
            string mk = f["pass"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if (tv != null)
            {
                var lstquyen = db.LoaiTV_Quyens.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                foreach (var item in lstquyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["TaiKhoan"] = tv;
                if (tv.MaLoaiTV == 3) { return RedirectToAction("Index");}
                else { return RedirectToAction("QuanLySP","ADQuanLySP"); }
                
            }
            return Content("Tài khoản hoặc mật khẩu không đúng");
        }
        public void PhanQuyen(string tk, string q)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, tk, DateTime.Now, DateTime.Now.AddHours(3), false, q, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}