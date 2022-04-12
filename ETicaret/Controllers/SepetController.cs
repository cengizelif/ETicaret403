using ETicaret.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.Controllers
{
    public class SepetController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
  
        public ActionResult Index()
        {
            return View();
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
    }
}