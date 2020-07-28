using Rezultati.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Rezultati.Models;

namespace Rezultati.Controllers
{
    public class TimCRUDController : Controller
    {
        // GET: Test
        public ActionResult Index()
        { 
            return View();
        }

        [HttpPost]
        public JsonResult TimoviList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new RezultatiContext())
                {
                    var timovi = context.Tims.Select(t => new { t.TimID, t.Naziv, t.Grad, t.Stadion, t.Trener });

                    int timoviCount = timovi.Count();
                    var records = timovi.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();

                    return Json(new { Result = "OK", Records = records, TotalRecordCount = timoviCount });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(TimViewModel tim)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new RezultatiContext())
                {
                    var noviTim = new Tim { Naziv = tim.Naziv, Grad = tim.Grad, Stadion = tim.Stadion, TimID = tim.TimID, Trener = tim.Trener };
                    context.Tims.Add(noviTim);
                    context.SaveChanges();
                    tim.TimID = noviTim.TimID;
                    return Json(new { Result = "OK", Record = noviTim });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(TimViewModel tim)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new RezultatiContext())
                {
                    Tim updateTim = context.Tims.Find(tim.TimID);

                    updateTim.Naziv = tim.Naziv;
                    updateTim.Stadion = tim.Stadion;
                    updateTim.Grad = tim.Grad;
                    updateTim.Trener = tim.Trener;
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
        public JsonResult Delete(int timID)
        {
            try
            {
                using (var context = new RezultatiContext())
                {
                    var brisi = context.Tims.Find(timID);
                    context.Tims.Remove(brisi);
                    context.SaveChanges();
                    return Json(new { Result = "OK" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "Podatak je strani ključ u drugoj tabeli! Brisanje nije moguće!" });
            }
        }

        
    }
}