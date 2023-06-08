using pmihalic_zadaca_3.klase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.ChainOfResponsibility
{
    public class RaspodjelaBrod1 : LanacRaspodjelaPutnika
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

            if(putnici.getBrojPutnika() > zbrojPetNajvecih(brodovi))
            {
                Console.WriteLine("Broj putnika je veći od zbroja kapaciteta 5 najvećih brodova, nije moguće raspodjeliti");
                this.lanac.raspodjeli(new Putnici(0), brodovi);
            }
            else { 

            najveciBrod = NajveciBrod(brodovi);

            if(putnici.getBrojPutnika() >= najveciBrod.Kapacitet_putnika)
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
        }

        public void setNextChain(LanacRaspodjelaPutnika sljedeciLanac)
        {
            this.lanac = sljedeciLanac;
        }

        public int zbrojPetNajvecih(List<Brod> lista)
        {
            int zbroj = 0;
            int najveciKapacitet = 0;
            Brod najveciBrod = new Brod();
            List<Brod> brodoviList = new List<Brod>();
            foreach(Brod brod in lista)
            {
                brodoviList.Add(brod);
            }
            for (int i = 0; i < 5; i++)
            {
                najveciBrod = NajveciBrod(brodoviList);
                zbroj += najveciBrod.Kapacitet_putnika;
                brodoviList.Remove(najveciBrod);
            }
            return zbroj;
        }

        public Brod NajveciBrod(List<Brod> brodovi)
        {
            Brod najveciBrod = new Brod();
            int najveciKapacitet = 0;
            foreach (Brod brod in brodovi)
            {
                if (najveciKapacitet < brod.Kapacitet_putnika)
                {
                    najveciBrod = brod;
                    najveciKapacitet = brod.Kapacitet_putnika;
                }
            }
            return najveciBrod;
        }
    }
}
