using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trang_web_ban_dien_thoai.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;


namespace Trang_web_ban_dien_thoai.Areas.admin.Controllers
{
    public class adminSPController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: admin/adminSP
        public ActionResult Index()
        {
            var SP = db.SanPhams.ToList();
            return View(SP);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.TenSanPham = new SelectList(db.SanPhams.ToList().OrderBy(n => n.TenSanPham), "SanPham", "TenSanPham");
            ViewBag.MaSanPham = new SelectList(db.SanPhams.ToList().OrderBy(n => n.MaSanPham), "SanPham", "MaSanPham");
            return View();
        }

    }
}