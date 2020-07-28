using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class UcinakViewModel
    {
        public int IgracID { get; set; }
        public int UtakmicaID { get; set; }
        public int OdigranihMinuta { get; set; }
        public int Golova { get; set; }
        public int ZutihKartona { get; set; }
        public int CrvenihKartona { get; set; }
    }
}