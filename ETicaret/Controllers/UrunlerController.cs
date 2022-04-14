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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ETicaret.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UrunlerController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public void menuAyar()
        {
            if (isAdminUser())
            {
                ViewBag.display = "block";
            }
            else
            {
                ViewBag.display = "None";
            }
        }
        public ActionResult Index()
        {
            menuAyar();
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
