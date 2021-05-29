using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvekŠpraljaJakovZavrsniRad
{
    public class Racun
    {
        static int brojRac = 0;
        double total;
        DateTime datum;
        int broj;

        public Racun(double total, DateTime datum)
        {
            this.Total = total;
            this.Datum = datum;
            this.Broj = brojRac++;
        }

        public double Total { get => total; set => total = value; }
        public DateTime Datum { get => datum; set => datum = value; }
        public int Broj { get => broj; set => broj = value; }

        public override string ToString()
        {
            string ispis = "Racun broj: " + Broj + "\r\n" + "Datum: " + Datum + "\r\n" + "Total: " + Total + " kn";
            return ispis;
        }

    }
}
