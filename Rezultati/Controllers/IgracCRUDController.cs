using Rezultati.DBModels;
using Rezultati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Rezultati.Controllers
{
    public class IgracCRUDController : Controller
    {
        // GET: IgracCRUD
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult IgraciList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new RezultatiContext())
                {
                    var igraci = context.Igracs.Select(i => new
                    {
                        i.IgracID,
                        i.Ime,
                        i.Prezime,
                        i.TimID,
                        i.Pozicija,
                        i.GradRodjenja,
                        i.BrojDresa,
                        i.DatumRodjenja,
                        i.DrzavaRodjenja
                    });

                    int igraciCount = igraci.Count();
                    var records = igraci.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();

                    return Json(new { Result = "OK", Records = records, TotalRecordCount = igraciCount });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetTimOptions()
        {
            try
            {
                using (var context = new RezultatiContext())
                {
                    var timovi = context.Tims.Select(t => new { DisplayText = t.Naziv, Value = t.TimID }).ToList();
                    return Json(new { Result = "OK", Options = timovi });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(IgracViewModel igrac)
        {
            if (!BrojDresaIsValid(igrac.BrojDresa, igrac.TimID))
                return Json(new { Result = "ERROR", Message = "Broj dresa već postoji!" });
            try
            {
                using (var context = new RezultatiContext())
                {
                    var noviIgrac = new Igrac
                    {
                        Ime = igrac.Ime,
                        GradRodjenja = igrac.GradRodjenja,
                        BrojDresa = igrac.BrojDresa,
                        TimID = igrac.TimID,
                        DatumRodjenja = igrac.DatumRodjenja,
                        DrzavaRodjenja = igrac.DrzavaRodjenja,
                        Pozicija = igrac.Pozicija,
                        Prezime = igrac.Prezime
                    };

                    context.Igracs.Add(noviIgrac);
                    context.SaveChanges();
                    igrac.IgracID = noviIgrac.IgracID;
                    return Json(new { Result = "OK", Record = noviIgrac });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public bool BrojDresaIsValid(int brDresa, int idTima)
        {
            using (var context = new RezultatiContext())
            {
                return context.Igracs.Where(i => i.TimID == idTima).Where(i => i.BrojDresa == brDresa).Count() == 0 ? true : false;
            }
        }

        [HttpPost]
        public JsonResult Update(IgracViewModel igrac)
        {
            if (!BrojDresaIsValid(igrac.BrojDresa, igrac.TimID))
            {
                using (var context = new RezultatiContext())
                {
                    try
                    {
                        if (context.Igracs.Where(i => i.TimID == igrac.TimID).Where(i => i.BrojDresa == igrac.BrojDresa).FirstOrDefault().IgracID != igrac.IgracID)
                            return Json(new { Result = "ERROR", Message = "Broj dresa već postoji!" });
                    }
                    catch (Exception) { }
                }
            }

            try
            {
                using (var context = new RezultatiContext())
                {
                    Igrac updateIgrac = context.Igracs.Find(igrac.IgracID);

                    updateIgrac.Ime = igrac.Ime;
                    updateIgrac.Prezime = igrac.Prezime;
                    updateIgrac.GradRodjenja = igrac.GradRodjenja;
                    updateIgrac.DrzavaRodjenja = igrac.DrzavaRodjenja;
                    updateIgrac.BrojDresa = igrac.BrojDresa;
                    updateIgrac.Pozicija = igrac.Pozicija;
                    updateIgrac.TimID = igrac.TimID;
                    updateIgrac.DatumRodjenja = igrac.DatumRodjenja;
                    context.SaveChanges();

                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int igracID)
        {
            try
            {
                using (var context = new RezultatiContext())
                {
                    var brisi = context.Igracs.Find(igracID);
                    context.Igracs.Remove(brisi);
                    context.SaveChanges();
                    return Json(new { Result = "OK" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "Podatak je strani ključ u drugoj tabeli! Brisanje nije moguće!" });
            }
        }
      
        public ActionResult DetaljiIgraca(string igracid)
        {
            using (var context = new RezultatiContext())
            {
                var igrac = context.Igracs.Single(i => i.IgracID.ToString() == igracid);
                var igracViewModel = new IgracViewModel()
                {
                    BrojDresa = igrac.BrojDresa,
                    DatumRodjenja = igrac.DatumRodjenja,
                    DrzavaRodjenja = igrac.DrzavaRodjenja,
                    GradRodjenja = igrac.GradRodjenja,
                    Ime = igrac.Ime,
                    Prezime = igrac.Prezime,
                    Pozicija = igrac.Pozicija
                };
                ViewBag.nazivTima = igrac.Tim.Naziv;
                ViewBag.ukupnoGolova = context.Ucinaks.Where(u => u.IgracID == igrac.IgracID).Sum(u => u.Golova).ToString();
                ViewBag.ukupnoZutih = context.Ucinaks.Where(u => u.IgracID == igrac.IgracID).Sum(u => u.ZutihKartona).ToString();
                ViewBag.ukupnoCrvenih = context.Ucinaks.Where(u => u.IgracID == igrac.IgracID).Sum(u => u.CrvenihKartona).ToString();
                ViewBag.minutaza = Math.Round(context.Ucinaks.Where(u => u.IgracID == igrac.IgracID).Average(u => u.OdigranihMinuta), 2).ToString();
                
                return View("DetaljiIgraca", igracViewModel);
            }
                
        }
    }
}