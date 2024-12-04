using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trang_web_ban_dien_thoai.Areas.admin.Model;
using Trang_web_ban_dien_thoai.Models;

namespace Trang_web_ban_dien_thoai.Areas.admin.Controllers
{
    public class DonHangsController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/DonHangs
        public ActionResult Index()
        {
            var donHangs = db.DonHangs.Include(d => d.KhachHang);
            return View(donHangs.ToList());
        }

        // GET: admin/DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            
            var query = from orderDetail in db.ChiTietDonHangs
                        join product in db.SanPhams on orderDetail.MaSanPham equals product.MaSanPham
                        where orderDetail.idDonHang == id
                        select new DetailOrderViewModel()
                        {
                            TenSP = product.TenSanPham,
                            HinhAnh = product.HinhChinh,
                            SoLuong = orderDetail.SoLuong,
                            DonGia = orderDetail.Gia
                       
                        };

            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(query.ToList());
        }

        // GET: admin/DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.idKH = new SelectList(db.KhachHangs, "idKH", "TenKH");
            return View();
        }

        // POST: admin/DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDonHang,NgayDat,NgayGiao,idKH,TrangThaiDatHang,TrangThaiThanhToan")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idKH = new SelectList(db.KhachHangs, "idKH", "TenKH", donHang.idKH);
            return View(donHang);
        }

        // GET: admin/DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.idKH = new SelectList(db.KhachHangs, "idKH", "TenKH", donHang.idKH);
            return View(donHang);
        }

        // POST: admin/DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDonHang,NgayDat,NgayGiao,idKH,TrangThaiDatHang,TrangThaiThanhToan")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idKH = new SelectList(db.KhachHangs, "idKH", "TenKH", donHang.idKH);
            return View(donHang);
        }

        // GET: admin/DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: admin/DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
