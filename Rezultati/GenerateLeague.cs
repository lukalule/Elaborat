using Rezultati.DBModels;
using Rezultati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rezultati
{
    public sealed class GenerateLeague
    {
        static GenerateLeague()
        {

        }
        private GenerateLeague()
        {

        }
        public static GenerateLeague GetInstance { get; } = new GenerateLeague();

        private static CryptoRandom rnd = new CryptoRandom();



        public static List<KoloViewModel> NewLeague()
        {
            List<TimViewModel> timovi;
            
            using (var context = new RezultatiContext())
            {
                timovi = context.Tims.Select(t => new TimViewModel
                {
                    TimID = t.TimID,
                    Grad = t.Grad,
                    Naziv = t.Naziv,
                    Stadion = t.Stadion,
                    Trener = t.Trener
                }).ToList();
            }

            if ((int)Math.Ceiling(Math.Log(timovi.Count) / Math.Log(2)) != (int)Math.Floor(Math.Log(timovi.Count) / Math.Log(2)))
                throw new Exception("Format lige mora biti tipa 2^n!");

            int[] timoviID = new int[timovi.Count]; //za izvlacenje
            for (int i = 0; i < timovi.Count; i++)            
                timoviID[i] = timovi.ElementAt(i).TimID;
            
            int brKola = BrKombinacijaUtakmica(timovi.Count) / 2;
            int brUtakmicaUKolu = (BrKombinacijaUtakmica(timovi.Count)*2) / brKola;
            List<Par> listaParova = GetParove(timoviID);
            List<KoloViewModel> listaKola = new List<KoloViewModel>();

            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            for (int i = 0; i < brKola; i++)
            {
                List<UtakmicaViewModel> listaUtakmica = new List<UtakmicaViewModel>();
                for (int j = 0; j < brUtakmicaUKolu; j++)
                {
                    UtakmicaViewModel utakmica = new UtakmicaViewModel();
                    utakmica.DomaciTimID = listaParova[i * brUtakmicaUKolu + j].DomacinID;
                    utakmica.GostujuciTimID = listaParova[i * brUtakmicaUKolu + j].GostID;
                    utakmica.Kolo = i + 1;
                    DateTime datumIgranja = start.AddDays(rnd.Next(1, DateTime.DaysInMonth(start.Year, start.Month) + 1));
                    utakmica.Datum = new DateTime(datumIgranja.Year, datumIgranja.Month, datumIgranja.Day, rnd.Next(18, 23), 0, 0);
                    utakmica.UtakmicaID = i * brUtakmicaUKolu + j;
                    utakmica.Odigrana = false;
                    listaUtakmica.Add(utakmica);
                }
                KoloViewModel novoKolo = new KoloViewModel();
                novoKolo.Utakmice = listaUtakmica;
                listaKola.Add(novoKolo);
                start = start.AddMonths(1);
            }

            return listaKola;
        }

        private static List<Par> UrediParove(List<Par> listaParova)
        {
            int brUtakmica = BrKombinacijaUtakmica(8) * 2;
            Par[] niz = new Par[brUtakmica];
            for (int k = 0; k < niz.Length; k++)
            {
                niz[k] = new Par(0, 0);
            }


            for (int i = 0; i < listaParova.Count; i++)
            {
                for (int j = 0; j < niz.Length; j++)
                {

                    if (niz[j].DomacinID != listaParova[i].DomacinID && niz[j].GostID != listaParova[i].GostID
                        && niz[j].DomacinID != listaParova[i].GostID && niz[j].GostID != listaParova[i].DomacinID
                        && MozeLiUKolo(j, niz, listaParova[i]) && MozeLiUopste(listaParova[i], niz) && niz[j].DomacinID == 0 && niz[j].GostID == 0)
                    {
                        niz[j] = new Par(listaParova[i].DomacinID, listaParova[i].GostID);
                        break;
                    }

                }
            }
            return niz.ToList();
        }

        private static bool MozeLiUKolo(int j, Par[] niz, Par par)
        {
            int kolo = (int)((double)j / 4);
            bool exists = false;
            for (int i = kolo * 4; i < kolo * 4 + 4; i++)
            {
                if (niz[i].DomacinID == par.DomacinID || niz[i].GostID == par.GostID || niz[i].GostID == par.DomacinID || niz[i].DomacinID == par.GostID)
                    exists = true;
            }
            return exists ? false : true;
        }

        private static bool MozeLiUopste(Par par, Par[] niz)
        {
            bool exists = false;
            for (int i = 0; i < niz.Length; i++)
            {
                if (niz[i].DomacinID == par.DomacinID && niz[i].GostID == par.GostID)
                    exists = true;
            }
            return exists ? false : true;
        }
        private static List<Par> GetParove(int[] nizID)
        {
            int brUtakmica = BrKombinacijaUtakmica(nizID.Length) * 2;
            int[] randNiz = RandomNiz(8, 8);
            int[] mixNiz = new int[8];
            for (int i = 0; i < nizID.Length; i++)
            {
                mixNiz[i] = nizID[randNiz[i]];
            }
            nizID = mixNiz;
            Par[] parovi = new Par[brUtakmica];

            int brParova = 0;
            while (brParova != brUtakmica)
            {
                bool parExists = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        parExists = false;
                        if (i == j) continue;

                        for (int k = 0; k < brParova; k++)
                        {
                            if (parovi[k].DomacinID == nizID[i] && parovi[k].GostID == nizID[j])
                                parExists = true;
                        }

                        if (!parExists)
                        {
                            parovi[brParova++] = (new Par(nizID[i], nizID[j]));
                        }
                    }
                }
            }
            return UrediParove(parovi.ToList());
        }


        public static int Faktorijel(int n)
        {
            int proizvod = 1;
            for (int i = 1; i <= n; i++)
                proizvod *= i;
            return proizvod;
        }

        public static int BrKombinacijaUtakmica(int brTimova) //bez ponavljanja
        {
            return Faktorijel(brTimova) / (2 * Faktorijel(brTimova - 2));
        }


        public static int[] RandomNiz(int brojElemenata, int opseg)
        {
            int[] niz = new int[brojElemenata];

            int br = 0;
            niz[0] = rnd.Next(0, opseg);
            for (int i = 1; i < brojElemenata;)
            {
                br = 0;

                int randbr = rnd.Next(0, opseg);

                for (int j = 0; j < i; j++)
                    if (niz[j] == randbr)
                        br++;

                if (br == 0)
                {
                    niz[i] = randbr;
                    i++;
                }

            }
            return niz;
        }

        public static void GenerisiUcinke()
        {
            using (var context = new RezultatiContext())
            {
                var listaUtakmica = context.Utakmicas.ToList();

                foreach (var utakmica in listaUtakmica)
                {
                    var domacin = utakmica.Tim;
                    foreach (var igrac in domacin.Igracs)
                    {
                        var ucinak = new Ucinak();
                        ucinak.IgracID = igrac.IgracID;
                        ucinak.UtakmicaID = utakmica.UtakmicaID;
                        ucinak.CrvenihKartona = 0;
                        ucinak.ZutihKartona = 0;
                        ucinak.OdigranihMinuta = 0;
                        ucinak.Golova = 0;
                        context.Ucinaks.Add(ucinak);
                    }

                    var gost = utakmica.Tim1;
                    foreach (var igrac in gost.Igracs)
                    {
                        var ucinak = new Ucinak();
                        ucinak.IgracID = igrac.IgracID;
                        ucinak.UtakmicaID = utakmica.UtakmicaID;
                        ucinak.CrvenihKartona = 0;
                        ucinak.ZutihKartona = 0;
                        ucinak.OdigranihMinuta = 0;
                        ucinak.Golova = 0;
                        context.Ucinaks.Add(ucinak);
                    }
                }
                context.SaveChanges();
            }
        }

        public static void GenerisiLigu(List<KoloViewModel> listaKola)
        {
            using (var context = new RezultatiContext())
            {
                foreach (var kolo in listaKola)
                {
                    foreach (var utakmica in kolo.Utakmice)
                    {
                        context.Utakmicas.Add(new Utakmica()
                        {
                            Datum = utakmica.Datum,
                            DomaciTimID = utakmica.DomaciTimID,
                            GostujuciTimID = utakmica.GostujuciTimID,
                            Kolo = utakmica.Kolo,
                            Odigrana = utakmica.Odigrana,
                            DomacinGolovi = utakmica.DomacinGolovi,
                            GostGolovi = utakmica.GostGolovi
                        });
                    }
                }
                context.SaveChanges();
            }
        }

    }
    
}