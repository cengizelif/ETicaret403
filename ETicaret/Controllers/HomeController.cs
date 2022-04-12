using ETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
        public ActionResult Index()
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
            ViewBag.SonUrunler = db.Urunler.OrderByDescending(x => x.UrunId).Take(10).ToList();
            return View();
        }

        public ActionResult Kategori(int id)
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
            ViewBag.Kategori = db.Kategoriler.Find(id);
            return View(db.Urunler.Where(x=>x.KategoriId==id).ToList());
        }

        public ActionResult Urun(int id)
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
        
            return View(db.Urunler.Find(id));
        }
    }
}