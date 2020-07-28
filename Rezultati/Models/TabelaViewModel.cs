using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class TabelaViewModel
    {
        public string NazivTima { get; set; }
        public int? BrPobijeda { get; set; }
        public int? BrPoraza { get; set; }
        public int? BrNerjesenih { get; set; }
        public int? GolRazlika { get; set; }
        public int? Bodovi { get; set; }

      
    }
}