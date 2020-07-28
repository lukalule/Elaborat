using Rezultati.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class UtakmicaViewModel
    {
        public int UtakmicaID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        public int Kolo { get; set; }
        public int DomaciTimID { get; set; }
        public int GostujuciTimID { get; set; }
        public bool Traje { get; set; }
        public bool Odigrana { get; set; }
        public int DomacinGolovi { get; set; }
        public int GostGolovi { get; set; }
      
    }
}