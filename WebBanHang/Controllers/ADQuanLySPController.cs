using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ADQuanLySPController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: ADQuanLySP
        public ActionResult QuanLySP()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCCs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {

            ViewBag.MaNCC = new SelectList(db.NhaCCs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //Ktra Hinh anh
            if (HinhAnh.ContentLength > 0)
            {
                var filename = Path.GetFileName(HinhAnh.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/HinhSP/"), filename);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = filename;


                }


            }
            db.SanPhams.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("QuanLySP");

        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCCs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
           

            ViewBag.MaNCC = new SelectList(db.NhaCCs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            SanPham sp = db.SanPhams.Single(n => n.MaSP == model.MaSP);
            sp.TenSP = model.TenSP;
            sp.SoLuongTon = model.SoLuongTon;
            sp.NgayCapNhat = model.NgayCapNhat;
            sp.Moi = model.Moi;
            sp.MoTa = model.MoTa;
            sp.CauHinh = model.CauHinh;
            sp.DonGia = model.DonGia;
            
            db.SubmitChanges();
            return RedirectToAction("QuanLySP");
            
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            {
                if (id == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                if (sp == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MaNCC = new SelectList(db.NhaCCs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
                ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
                return View(sp);
            }
        }
        [HttpPost]
        public ActionResult Xoa(int? id,FormCollection f)
        {
            {
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                if (sp == null)
                {
                    return HttpNotFound();
                }
                db.SanPhams.DeleteOnSubmit(sp);
                db.SubmitChanges(); ;
                return RedirectToAction("QuanLySP");

            }
        }
    }
}