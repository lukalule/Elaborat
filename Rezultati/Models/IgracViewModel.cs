using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rezultati.Models
{
    public class IgracViewModel
    {
        public int IgracID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }
        public string DrzavaRodjenja { get; set; }
        public string GradRodjenja { get; set; }
        public int BrojDresa { get; set; }
        public string Pozicija { get; set; }
        public int TimID { get; set; }
    }
}