using ETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.Controllers
{
    public class SiparisController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
        public ActionResult Index()
        {
           var siparis= db.Siparis.ToList();
            return View(siparis.ToList());
        }

        public ActionResult SiparisDetay(int id)
        {
            var siparisdetay = db.SiparisDetay.Where(x => x.SiparisId == id).ToList();
            return View(siparisdetay.ToList());
        }
    }
}