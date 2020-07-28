using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati
{
    public class Par
    {
        public int DomacinID { get; set; }
        public int GostID { get; set; }

        public Par() { }

        public Par(int domacin, int gost)
        {
            DomacinID = domacin;
            GostID = gost;
        }
    }
}