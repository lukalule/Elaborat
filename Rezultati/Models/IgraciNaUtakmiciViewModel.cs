using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class IgraciNaUtakmiciViewModel
    {
        public List<IgracViewModel> IgraciDomacin { get; set; }
        public List<IgracViewModel> IgraciGost { get; set; }
        public int UtakmicaID { get; set; }
    }
}