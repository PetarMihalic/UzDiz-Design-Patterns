using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_1.klase
{
    public class Zahtjev_rezervacije
    {
        public int Id_brod { get; set; }
        public DateTime Datum_vrijeme_od { get; set; }
        public int Trajanje_priveza_u_h { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"{Id_brod} {Datum_vrijeme_od} {Trajanje_priveza_u_h} {Status}";
        }

        private Zahtjev_rezervacije(ZahtjevBuilder builder)
        {
            this.Id_brod = builder.Id_brod;
            this.Datum_vrijeme_od = builder.Datum_vrijeme_od;
        }

        public class ZahtjevBuilder
        {
            public int Id_brod;
            public DateTime Datum_vrijeme_od;
            public int Trajanje_priveza_u_h;
            public string Status;

            public ZahtjevBuilder(int id_brod, DateTime datum_vrijeme_od)
            {
                this.Id_brod = id_brod;
                this.Datum_vrijeme_od = datum_vrijeme_od;
            }

            public ZahtjevBuilder setStatus(string s)
            {
                this.Status = s;
                return this;
            }

            public ZahtjevBuilder setTrajanjePriveza(int sati)
            {
                this.Trajanje_priveza_u_h = sati;
                return this;
            }

            public Zahtjev_rezervacije build()
            {
                return new Zahtjev_rezervacije(this);
            }
        }
    }
}
