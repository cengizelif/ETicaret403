using ETicaret.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.Controllers
{
    public class SepetController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
  
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var sepet = db.Sepet.Where(x => x.UserId == userID).Include(s=>s.Urunler);

            return View(sepet.ToList());
        }
        public ActionResult SepeteEkle(int? adet,int id)
        {
           string userID=User.Identity.GetUserId();

           Sepet sepettekiurun=db.Sepet.FirstOrDefault(x => x.UrunId == id && x.UserId == userID);

            Urunler urun = db.Urunler.Find(id);

            if(sepettekiurun==null)
            {
                Sepet yeniurun = new Sepet()
                {
                    UserId=userID,
                    UrunId=id,
                    Adet=adet??1,
                    ToplamTutar=(adet ?? 1)*urun.UrunFiyati
                };
                db.Sepet.Add(yeniurun);
            }
            else
            {
                sepettekiurun.Adet = sepettekiurun.Adet + (adet ?? 1);
                sepettekiurun.ToplamTutar = sepettekiurun.Adet * urun.UrunFiyati;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
   
        public ActionResult SepetGuncelle(int? adet,int id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sepet sepet = db.Sepet.Find(id);
            if(sepet==null)
            {
                return HttpNotFound();
            }

            Urunler urun = db.Urunler.Find(sepet.UrunId);

            sepet.Adet = adet ?? 1;
            sepet.ToplamTutar = sepet.Adet * urun.UrunFiyati;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Sil(int id)
        {
            Sepet sepet = db.Sepet.Find(id);
            db.Sepet.Remove(sepet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}