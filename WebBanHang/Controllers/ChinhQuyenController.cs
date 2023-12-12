using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    [Authorize(Roles = "DangKy")]
    public class ChinhQuyenController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: ChinhQuyen
        public ActionResult Index()
        {
            return View(db.LoaiTVs.OrderBy(n => n.TenLoai));
        }

        [HttpGet]
        public ActionResult ChinhQuyen(int? id)
        {
            if(id==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiTV ltv = db.LoaiTVs.SingleOrDefault(n => n.MaLoaiTV == id);
            if(ltv==null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQuyen = db.Quyens;
            ViewBag.LoaiTVQuyen = db.LoaiTV_Quyens.Where(n => n.MaLoaiTV == id);
            return View(ltv);

        }
        [HttpPost]
        public ActionResult ChinhQuyen(int? MaLTV,IEnumerable<LoaiTV_Quyen> lstQuyen)
        {
            var lstDaPQ = db.LoaiTV_Quyens.Where(n => n.MaLoaiTV == MaLTV);
            if(lstDaPQ != null)
            {

                db.LoaiTV_Quyens.DeleteAllOnSubmit(lstDaPQ);
                db.SubmitChanges();
            }    
            foreach(var item in lstQuyen)
            {
                item.MaLoaiTV = int.Parse(MaLTV.ToString());
                
                db.LoaiTV_Quyens.InsertOnSubmit(item);
                   
            }
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}