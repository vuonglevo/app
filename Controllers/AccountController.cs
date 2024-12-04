using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trang_web_ban_dien_thoai.Models;

namespace Trang_web_ban_dien_thoai.Controllers
{
    public class AccountController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Account
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(KhachHang cus)
        {
            var checklogin = db.KhachHangs.SingleOrDefault(c => c.Username == cus.Username && c.Password == cus.Password);
            var checkadmin = db.TaiKhoans.SingleOrDefault(x => x.Username == cus.Username && x.Pass == cus.Password);
            if (checklogin != null)
            {
                Session["KhachHang"] = checklogin;
                return RedirectToAction("Index", "Home");
            }
            else if (checkadmin != null)
            {
                Session["KhachHang"] = checkadmin;
                
                return RedirectToAction("index", "adminsp", new { area = "admin" });
            }
            else
            {
                ViewBag.ThongBao = "Sai thong tin tai khoan";
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang cus)
        {
            db.KhachHangs.Add(cus);
            db.SaveChanges();
            return RedirectToAction("DangNhap", "Account");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}