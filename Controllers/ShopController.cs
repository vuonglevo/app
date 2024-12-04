using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trang_web_ban_dien_thoai.Models;
using System.Data.Entity;
using PagedList;

namespace Trang_web_ban_dien_thoai.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        ShopOnlineEntities db = new ShopOnlineEntities();
        public ActionResult Index(int? page)
        {
            if(page==null)
            {
                page = 1;
            }
            var DSsanpham = (from SanPham in db.SanPhams select SanPham).OrderBy(x => x.MaSanPham);
            int pagesize = 8;
            int pagenumber = (page ?? 1);
            ViewBag.pageSize = pagesize;
            List<SanPham> sp = db.SanPhams.ToList();
            return View(sp.ToPagedList(pagenumber,pagesize));
        }

        // GET: Shop/Details/5
        public ActionResult Details(int id)
        {
            var detail = db.SanPhams.Where(n => n.MaSanPham == id).FirstOrDefault();
            return View(detail);
        }

        // GET: Shop/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shop/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shop/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Shop/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shop/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Shop/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Nhanhieu(int? idnhanhieu)
        {
            var nhanhieu = db.SanPhams.Where(n => n.MaNhaSanXuat == idnhanhieu).ToList();
            return View(nhanhieu);
        }

        public ActionResult Search(string keyword)
        {
            var iteams = db.SanPhams.Where(x => x.TenSanPham.Contains(keyword)).ToList();
            ViewBag.keyWord = keyword;
            return View(iteams);
        }

    }
}
