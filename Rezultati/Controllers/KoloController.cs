using Rezultati.DBModels;
using Rezultati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Rezultati.Controllers
{
    
    public sealed class KoloController : Controller
    {               
        // GET: Kolo
        public ActionResult Index()
        {
            var optionsKola = new List<SelectListItem>();

            for (int i = 1; i <= 14; i++)
            {
                optionsKola.Add(new SelectListItem()
                {
                    Text = i + ". Kolo",
                    Value = i.ToString()
                });
            }
            ViewBag.optionsKola = optionsKola;
                return View();
        }

        public JsonResult NadjiUcinak(int utakmicaid, int igracid)
        {
            using (var context = new RezultatiContext())
            {
                var ucinak = context.Ucinaks.SingleOrDefault(u => u.UtakmicaID == utakmicaid && u.IgracID == igracid);
                if(ucinak is null)
                {
                    //ucinak = new Ucinak
                    //{
                    //    CrvenihKartona = 0,
                    //    Golova = 0,
                    //    IgracID = igracid,
                    //    UtakmicaID = utakmicaid,
                    //    ZutihKartona = 0,
                    //    OdigranihMinuta = 0
                    //};
                }
                return Json(new { golova = ucinak.Golova, minuta = ucinak.OdigranihMinuta,
                                  zutih = ucinak.ZutihKartona, crvenih = ucinak.CrvenihKartona, utakmicaid, igracid}, 
                                  JsonRequestBehavior.AllowGet);
            }

        }

        public void PostUcinak(int utakmicaid, int igracid, int minuta, int golova, int zutih, int crvenih)
        {
            using (var context = new RezultatiContext())
            {
                var utakmica = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid);
                var ucinak = context.Ucinaks.Single(u => u.UtakmicaID == utakmicaid && u.IgracID == igracid);

                if (ucinak.Igrac.TimID == utakmica.DomaciTimID)
                {
                    utakmica.DomacinGolovi = context.Utakmicas.Single(ut => ut.UtakmicaID == utakmica.UtakmicaID)
                            .Ucinaks.Where(u => u.Igrac.TimID == utakmica.DomaciTimID).Sum(u => u.Golova) - ucinak.Golova;
                    utakmica.DomacinGolovi += golova;
                }
                else if (ucinak.Igrac.TimID == utakmica.GostujuciTimID) { 
                    utakmica.GostGolovi = context.Utakmicas.Single(ut => ut.UtakmicaID == utakmica.UtakmicaID)
                        .Ucinaks.Where(u => u.Igrac.TimID == utakmica.GostujuciTimID).Sum(u => u.Golova) - ucinak.Golova;
                    utakmica.GostGolovi += golova;
                }


                ucinak.OdigranihMinuta = minuta;
                ucinak.Golova = golova;
                ucinak.ZutihKartona = zutih;
                ucinak.CrvenihKartona = crvenih;
                if (zutih == 2)
                    ucinak.CrvenihKartona = 1;




                context.SaveChanges();              
            }
        }



        public ActionResult PodaciZaUtakmicu(int utakmicaid)
        {
            using (var context = new RezultatiContext())
            {              
                var igraciDomacin = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid)
                                    .Tim.Igracs.Select(i => new IgracViewModel()
                                    {
                                        Ime = i.Ime,
                                        Prezime = i.Prezime,
                                        BrojDresa = i.BrojDresa,
                                        DatumRodjenja = i.DatumRodjenja,
                                        DrzavaRodjenja = i.DrzavaRodjenja,
                                        GradRodjenja = i.GradRodjenja,
                                        Pozicija = i.Pozicija,
                                        TimID = i.TimID,
                                        IgracID = i.IgracID
                                    }).ToList();

                var igraciGost = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid)
                                    .Tim1.Igracs.Select(i => new IgracViewModel()
                                    {
                                        Ime = i.Ime,
                                        Prezime = i.Prezime,
                                        BrojDresa = i.BrojDresa,
                                        DatumRodjenja = i.DatumRodjenja,
                                        DrzavaRodjenja = i.DrzavaRodjenja,
                                        GradRodjenja = i.GradRodjenja,
                                        Pozicija = i.Pozicija,
                                        TimID = i.TimID,
                                        IgracID = i.IgracID
                                    }).ToList();

                var igraciNaUtakmici = new IgraciNaUtakmiciViewModel();
                igraciNaUtakmici.IgraciDomacin = igraciDomacin;
                igraciNaUtakmici.IgraciGost = igraciGost;
                igraciNaUtakmici.UtakmicaID = utakmicaid;

                ViewBag.Domacin = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid).Tim.Naziv;
                ViewBag.Gost = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid).Tim1.Naziv;

                return PartialView("_PodaciZaUtakmicu", igraciNaUtakmici);
            }
            
        }
        public JsonResult DetaljiUtakmice(int utakmicaid)
        {
            using (var context = new RezultatiContext())
            {
                var utakmica = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid);
                string datum = utakmica.Datum.Day.ToString() + "." + utakmica.Datum.Month.ToString() + "." + utakmica.Datum.Year.ToString();
                string grad = utakmica.Tim.Grad;
                string stadion = utakmica.Tim.Stadion;
                string domacin = utakmica.Tim.Naziv;
                string gost = utakmica.Tim1.Naziv;  //
                var listaStr = utakmica.Ucinaks.Where(u => u.Golova > 0);
                List<string> strijelci = new List<string>();
                foreach (var strijelac in listaStr)
                {
                    strijelci.Add("(" + strijelac.Igrac.Tim.Naziv + ")" + strijelac.Igrac.Ime + " " + strijelac.Igrac.Prezime + " - Pogodaka: " + strijelac.Golova);
                }
                strijelci.Sort();
                return Json(new { datum, grad, stadion, domacin, gost, strijelci }, JsonRequestBehavior.AllowGet);
            }
        }
        public void ZapocniUtakmicu(int utakmicaid)
        {
            using (var context = new RezultatiContext())
            {
                var utakmica = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid);
                utakmica.Traje = true;
                context.SaveChanges();
            }
        }

        public void ZavrsiUtakmicu(int utakmicaid)
        {
            using (var context = new RezultatiContext())
            {
                var utakmica = context.Utakmicas.Single(u => u.UtakmicaID == utakmicaid);
                if (utakmica.Odigrana)                
                    utakmica.Odigrana = false;           
                else
                    utakmica.Odigrana = true;

                    context.SaveChanges();           
            }
        }

        public PartialViewResult GetKolo(int brKola)
        {
            using (var context = new RezultatiContext())
            {
                
                var listaUtakmica = context.Utakmicas.Where(u => u.Kolo == brKola).Select(u => new UtakmicaViewModel()
                {
                    Datum = u.Datum,
                    Kolo = u.Kolo,
                    DomaciTimID = u.DomaciTimID,
                    GostujuciTimID = u.GostujuciTimID,
                    Odigrana = u.Odigrana,
                    Traje = u.Traje,
                    UtakmicaID = u.UtakmicaID,
                }).ToList();

                var listaImena = new List<ImenaTimova>();
                foreach (var utakmica in listaUtakmica)
                {
                    listaImena.Add(new ImenaTimova()
                    {
                        Domacin = context.Utakmicas.Single(u => u.UtakmicaID == utakmica.UtakmicaID).Tim.Naziv,
                        Gost = context.Utakmicas.Single(u => u.UtakmicaID == utakmica.UtakmicaID).Tim1.Naziv
                    });

                    utakmica.DomacinGolovi = context.Utakmicas.Single(ut => ut.UtakmicaID == utakmica.UtakmicaID)
                        .Ucinaks.Where(u => u.Igrac.TimID == utakmica.DomaciTimID).Sum(u => u.Golova);                    
                    utakmica.GostGolovi = context.Utakmicas.Single(ut => ut.UtakmicaID == utakmica.UtakmicaID)
                        .Ucinaks.Where(u => u.Igrac.TimID == utakmica.GostujuciTimID).Sum(u => u.Golova);
                }

                context.SaveChanges();
                ViewBag.listaImena = listaImena;
                return PartialView("_PartialGetKolo", listaUtakmica);
            }            
        }                       
       
    }
}