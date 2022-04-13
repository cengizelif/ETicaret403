using ETicaret.Models;
using Microsoft.AspNet.Identity;
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
        public ActionResult SiparisTamamla()
        {
            string userID = User.Identity.GetUserId();

List<Sepet> sepetUrunleri=db.Sepet.Where(x => x.UserId == userID).ToList();

            string ClientId = "1003001";//Bankanın verdiği magaza kodu
            string ToplamTutar = sepetUrunleri.Sum(x => x.ToplamTutar).ToString();
            string sipId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            string onayURL = "https://localhost:44376/Siparis/Tamamlandi";
            string hataURL = "https://localhost:44376/Siparis/Hatali";
            string RDN = "asdf";
            string StoreKey = "123456";

            string TransActionType = "Auth";
            string Instalment = "";

            string HashStr = ClientId + sipId + ToplamTutar + onayURL + hataURL + TransActionType + Instalment + RDN + StoreKey;//Bankanın istediği bilgiler

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = sipId;
            ViewBag.okUrl = onayURL;
            ViewBag.failUrl = hataURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = ToplamTutar;
            ViewBag.StoreType = "3d_pay_hosting"; // Ödeme modelimiz biz buna göre anlatıyoruz 
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.EMail = "cenelif@gmail.com";
            ViewBag.UserID = "ElifCengiz"; // bu id yi bankanın sanala pos ekranında biz oluşturuyoruz.
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";


            return View()

        }

    }
}