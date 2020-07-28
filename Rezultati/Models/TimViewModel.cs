using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class TimViewModel
    {
        public int TimID { get; set; }
        public string Naziv { get; set; }
        public string Grad { get; set; }
        public string Stadion { get; set; }
        public string Trener { get; set; }
    }
}