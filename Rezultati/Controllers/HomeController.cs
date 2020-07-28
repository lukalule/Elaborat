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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new RezultatiContext())
            {
                var tabela = context.Tims.Select(t => new TabelaViewModel()
                {
                    NazivTima = t.Naziv,
                    BrPobijeda = t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID && u.DomacinGolovi > u.GostGolovi).Count() + t.Utakmicas1.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID && u.DomacinGolovi < u.GostGolovi).Count(),
                    BrPoraza = t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID && u.DomacinGolovi < u.GostGolovi).Count() + t.Utakmicas1.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID && u.DomacinGolovi > u.GostGolovi).Count(),
                    BrNerjesenih = t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID && u.DomacinGolovi == u.GostGolovi).Count() + t.Utakmicas1.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID && u.DomacinGolovi == u.GostGolovi).Count(),
                    Bodovi = (t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID && u.DomacinGolovi > u.GostGolovi).Count() + t.Utakmicas1.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID && u.DomacinGolovi < u.GostGolovi).Count()) * 3
                            + t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID && u.DomacinGolovi == u.GostGolovi).Count() + t.Utakmicas1.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID && u.DomacinGolovi == u.GostGolovi).Count(),
                   
                    GolRazlika = (t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID).ToList().Sum(u => u.DomacinGolovi) == null? 0: t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID).ToList().Sum(u => u.DomacinGolovi))
                               + (t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID).ToList().Sum(u => u.GostGolovi) == null? 0: t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID).ToList().Sum(u => u.GostGolovi))
                               - (t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID).ToList().Sum(u => u.DomacinGolovi) == null? 0: t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.GostujuciTimID).ToList().Sum(u => u.DomacinGolovi))
                               - (t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID).ToList().Sum(u => u.GostGolovi) == null? 0: t.Utakmicas.Where(u => u.Odigrana && t.TimID == u.DomaciTimID).ToList().Sum(u => u.GostGolovi))

                }).ToList();
                tabela = tabela.OrderByDescending(t => t.Bodovi).OrderByDescending(t => t.GolRazlika).ToList();
                return View(tabela);
            }            
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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


        public ActionResult ListaTimova() 
        {
            using (var context = new RezultatiContext())
            {
                var timovi = context.Tims.Select(t => new TimViewModel
                {
                    TimID = t.TimID,
                    Grad = t.Grad,
                    Naziv = t.Naziv,
                    Stadion = t.Stadion,
                    Trener = t.Trener
                }).ToList();

                return View(timovi);
            }
        }

    }
}