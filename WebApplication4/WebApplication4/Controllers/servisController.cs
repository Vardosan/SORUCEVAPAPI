using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
using WebApplication4.ViewModel;
namespace WebApplication4.Controllers
{
    public class servisController : ApiController
    {
        DB03Entities db = new DB03Entities();
        SonucModel sonuc = new SonucModel();

        #region Ders

        [HttpGet]
        [Route("api/dersliste")]
        public List<DersModel> Dersliste()
        {
            List<DersModel> liste = db.Ders.Select(x => new DersModel()
            {
                DersId = x.DersId,
                DersAdi = x.DersAdi
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/dersbyid//{dersId}")]

        public DersModel DersById(string dersId)
        {
            DersModel kayit = db.Ders.Where(s => s.DersId == dersId).Select(x => new
           DersModel()
            {
                DersId = x.DersId,
                DersAdi = x.DersAdi
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/dersekle")]
        public SonucModel DersEkle(DersModel model)
        {
            if (db.Ders.Count(s => s.Derskodu == model.Derskodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ders Kodu Kayıtlıdır!";
                return sonuc;
            }

            Ders yeni = new Ders();
            yeni.DersId = Guid.NewGuid().ToString();
            yeni.Derskodu = model.Derskodu;
            yeni.DersAdi = model.DersAdi;
            db.Ders.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/dersduzenle")]
        public SonucModel DersDuzenle(DersModel model)
        {
            Ders kayit = db.Ders.Where(s => s.DersId == model.DersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.Derskodu = model.Derskodu;
            kayit.DersAdi = model.DersAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ders Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/derssil/{DersId}")]
        public SonucModel DersSil(string DersId)
        {
            Ders kayit = db.Ders.Where(s => s.DersId == DersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Kayit.Count(s => s.kayitDersId == DersId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Derse Kayıtlı Soru Olduğu İçin Ders Silinemez";
                return sonuc;
            }

            db.Ders.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }
        #endregion

        #region Soru

        [HttpGet]
        [Route("api/soruliste")]
        public List<SoruModel> SoruListe()
        {
            List<SoruModel> liste = db.Soru.Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                SoruNo = x.SoruNo,
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/sorubyıd/{soruId}")]
        public SoruModel SoruById(string soruId)
        {
            SoruModel kayit = db.Soru.Where(s => s.SoruId == soruId).Select(x =>
           new SoruModel()
           {
               SoruId = x.SoruId,
               SoruNo = x.SoruNo,
           }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/soruekle")]
        public SonucModel SoruEkle(SoruModel model)
        {
            if (db.Soru.Count(s => s.SoruNo == model.SoruNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Soru Numarası Kayıtlıdır!";
            }

            Soru yeni = new Soru();
            yeni.SoruId = Guid.NewGuid().ToString();
            yeni.SoruNo = model.SoruNo;
            db.Soru.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/soruduzenle")]
        public SonucModel SoruDuzenle(SoruModel model)
        {
            Soru kayit = db.Soru.Where(s => s.SoruId == model.SoruId).SingleOrDefault
                ();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.SoruNo = model.SoruNo;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/sorusil")]

        public SonucModel SoruSil(string SoruId)
        {
            Soru kayit = db.Soru.Where(s => s.SoruId == SoruId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Kayit.Count(s => s.kayitSoruId == SoruId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Soru Üzerinde Ders Kaydı Olduğu için Soru Silinemez!";
                return sonuc;
            }

            db.Soru.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Silindi";
            return sonuc;
        }
        #endregion

        #region Cevap

        [HttpGet]
        [Route("api/cevapliste")]
        public List<CevapModel> CevapListe()
        {
            List<CevapModel> liste = db.Cevap.Select(x => new CevapModel()
            {
                CevapId = x.CevapId,
                CevapNo = x.CevapNo,
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/cevapbyıd/{cevapId}")]
        public CevapModel CevapById(string cevapId)
        {
            CevapModel kayit = db.Cevap.Where(s => s.CevapId == cevapId).Select(x =>
           new CevapModel()
           {
               CevapId = x.CevapId,
               CevapNo = x.CevapNo,
           }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/cevapekle")]
        public SonucModel CevapEkle(CevapModel model)
        {
            if (db.Cevap.Count(s => s.CevapNo == model.CevapNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Cevap Numarası Kayıtlıdır!";
            }

            Cevap yeni = new Cevap();
            yeni.CevapId = Guid.NewGuid().ToString();
            yeni.CevapNo = model.CevapNo;
            db.Cevap.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/cevapduzenle")]
        public SonucModel CevapDuzenle(CevapModel model)
        {
            Cevap kayit = db.Cevap.Where(s => s.CevapId == model.CevapId).SingleOrDefault
                ();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.CevapNo = model.CevapNo;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/cevapsil")]

        public SonucModel CevapSil(string CevapId)
        {
            Cevap kayit = db.Cevap.Where(s => s.CevapId == CevapId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            db.Cevap.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap Silindi";
            return sonuc;
        }
        #endregion

        #region Kayit

        [HttpGet]
        [Route("api/DersSoruCevapListe/soruId")]
        public List<KayitModel> DersSoruCevapListe(string soruId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitSoruId == soruId).Select(x => new
                  KayitModel()
            {
                kayitId = x.kayitId,
                kayitDersId = x.kayitDersId,
                kayitSoruId = x.kayitSoruId,
                kayitCevapId = x.kayitCevapId,

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.soruBilgi = SoruById(kayit.kayitSoruId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);
                kayit.cevapBilgi = CevapById(kayit.kayitCevapId);

            }
            return liste;
        }

        [HttpGet]
        [Route("api/SoruDersCevapListe/dersId")]
        public List<KayitModel> SoruDersCevapListe(string dersId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitDersId == dersId).Select(x => new
                  KayitModel()
            {
                kayitId = x.kayitId,
                kayitDersId = x.kayitDersId,
                kayitSoruId = x.kayitSoruId,
                kayitCevapId = x.kayitCevapId,

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.soruBilgi = SoruById(kayit.kayitSoruId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);
                kayit.cevapBilgi = CevapById(kayit.kayitCevapId);
            }
            return liste;
        }

        [HttpPost]
        [Route("api/kayitekle")]

        public SonucModel KayitEkle(KayitModel model)
        {

            if (db.Kayit.Count(s => s.kayitDersId == model.kayitDersId && s.kayitSoruId == model.kayitSoruId && s.kayitCevapId == model.kayitCevapId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlgili Soru Derse Önceden Kayıtlıdır!";
                return sonuc;
            }


            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitSoruId = model.kayitSoruId;
            yeni.kayitDersId = model.kayitDersId;
            yeni.kayitCevapId = model.kayitCevapId;
            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Kaydı Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{kayitId}")]
        public SonucModel KayitSil(string kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Kaydı Silindi";

            return sonuc;
        }

        #endregion
    }
}

