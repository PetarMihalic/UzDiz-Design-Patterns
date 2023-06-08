using pmihalic_zadaca_2.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.klaseObjekata
{
    public class DnevnikRada
    {
        public int Id_brod { get; set; }
        public DateTime Datum_slanja_zahtjeva { get; set; }
        public Boolean odobren { get; set; }
        public DateTime Datum_vrijeme_od { get; set; }
        public DateTime Datum_vrijeme_do { get; set; }
        public Zahtjev_rezervacije zahtjev_Rezervacije { get; set; }

        public DnevnikRada(int id_brod, DateTime datum_slanja_zahtjeva, bool odobren, DateTime datum_vrijeme_od, DateTime datum_vrijeme_do, Zahtjev_rezervacije zahtjev_Rezervacije)
        {
            Id_brod = id_brod;
            Datum_slanja_zahtjeva = datum_slanja_zahtjeva;
            this.odobren = odobren;
            Datum_vrijeme_od = datum_vrijeme_od;
            Datum_vrijeme_do = datum_vrijeme_do;
            this.zahtjev_Rezervacije = zahtjev_Rezervacije;
        }
    }
}
