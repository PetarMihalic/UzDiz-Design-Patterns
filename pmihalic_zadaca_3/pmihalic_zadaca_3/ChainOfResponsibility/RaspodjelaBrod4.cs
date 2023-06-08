using pmihalic_zadaca_3.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.ChainOfResponsibility
{
    public class RaspodjelaBrod4 : LanacRaspodjelaPutnika
    {

        private LanacRaspodjelaPutnika lanac;
        public void raspodjeli(Putnici putnici, List<Brod> listaBrodova)
        {
            List<Brod> brodovi = new List<Brod>();
            foreach (Brod brod in listaBrodova)
            {
                brodovi.Add(brod);
            }
            Brod najveciBrod = null;
            int najveciKapacitet = 0;

            foreach (Brod brod in brodovi)
            {
                if(najveciKapacitet < brod.Kapacitet_putnika)
                {
                    najveciBrod = brod;
                    najveciKapacitet = brod.Kapacitet_putnika;
                }
            }

            if(putnici.getBrojPutnika() >= najveciKapacitet)
            {
                Console.WriteLine(najveciBrod.Kapacitet_putnika + " putnika raspoređeno na brod " + najveciBrod.Oznaka_broda + " - " + najveciBrod.Naziv);
                brodovi.Remove(najveciBrod);
                int ostatakPutnika = putnici.getBrojPutnika() - najveciBrod.Kapacitet_putnika;
                if(ostatakPutnika != 0) this.lanac.raspodjeli(new Putnici(ostatakPutnika), brodovi);
            }
            else
            {
                this.lanac.raspodjeli(putnici, brodovi);
            }
        }

        public void setNextChain(LanacRaspodjelaPutnika sljedeciLanac)
        {
            this.lanac = sljedeciLanac;
        }
    }
}
