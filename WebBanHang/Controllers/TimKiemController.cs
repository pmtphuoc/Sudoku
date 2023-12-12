using PagedList;
using System.Linq;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        BanHangDataContext db = new BanHangDataContext();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string tukhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int Pagesize = 3;
            int Pagenum = (page ?? 1);
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(tukhoa));
            ViewBag.TuKhoa = tukhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(Pagenum, Pagesize));
        }
        [HttpPost]
        public ActionResult TimKiemTu(string tukhoa)
        {

            return RedirectToAction("KQTimKiem", new { @tukhoa = tukhoa });
        }

    }
}