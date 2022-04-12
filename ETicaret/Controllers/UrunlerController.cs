using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETicaret.Models;

namespace ETicaret.Controllers
{
    public class UrunlerController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();

        // GET: Urunler
        public ActionResult Index()
        {
            var urunler = db.Urunler.Include(u => u.Kategoriler);
            return View(urunler.ToList());
        }

        // GET: Urunler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // GET: Urunler/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi");
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UrunId,UrunAdi,UrunAciklamasi,UrunFiyati,KategoriId")] Urunler urunler,HttpPostedFileBase UrunResmi)
        {
            if (ModelState.IsValid)
            {
                db.Urunler.Add(urunler);
                db.SaveChanges();
                if(UrunResmi!=null)
                {
                    string filepath = Path.Combine(Server.MapPath("~/Resim"), urunler.UrunId + ".jpg");
                    UrunResmi.SaveAs(filepath);
                }

                return RedirectToAction("Index");
            }

            ViewBag.KategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.KategoriId);
            return View(urunler);
        }

        // GET: Urunler/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.KategoriId);
            return View(urunler);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UrunId,UrunAdi,UrunAciklamasi,UrunFiyati,KategoriId")] Urunler urunler, HttpPostedFileBase UrunResmi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();
                if (UrunResmi != null)
                {
                    string filepath = Path.Combine(Server.MapPath("~/Resim"), urunler.UrunId + ".jpg");
                    UrunResmi.SaveAs(filepath);
                }
                return RedirectToAction("Index");
            }
            ViewBag.KategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.KategoriId);
            return View(urunler);
        }

        // GET: Urunler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: Urunler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urunler urunler = db.Urunler.Find(id);
            db.Urunler.Remove(urunler);
            db.SaveChanges();

            string filepath = Path.Combine(Server.MapPath("~/Resim"), urunler.UrunId + ".jpg");

            FileInfo fi = new FileInfo(filepath);

            if (fi.Exists)
              fi.Delete();

            return RedirectToAction("Index");
        }

      
    }
}
