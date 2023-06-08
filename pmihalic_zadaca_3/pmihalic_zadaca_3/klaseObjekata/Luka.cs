using pmihalic_zadaca_3.Composite;
using pmihalic_zadaca_3.IteratorUD;
using pmihalic_zadaca_3.klaseObjekata;
using pmihalic_zadaca_3.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.klase
{
    public class Luka : IBrodskaLukaComponent
    {
        public string Naziv { get; set; }
        public double GPS_sirina { get; set; }
        public double GPS_visina { get; set; }
        public int Dubina_luke { get; set; }
        public int Ukupni_broj_putnickih_vezova { get; set; }
        public int Ukupni_broj_poslovnih_vezova { get; set; }
        public int Ukupni_broj_ostalih_vezova { get; set; }
        public DateTime Virtualno_vrijeme { get; set; }

        public Collection kolekcijaMolova = new Collection();

        public Collection kolekcijaVezova = new Collection();
        public override string ToString()
        {
            return $"{Naziv} {GPS_sirina} {GPS_visina} {Dubina_luke} {Ukupni_broj_putnickih_vezova} {Ukupni_broj_poslovnih_vezova} {Ukupni_broj_ostalih_vezova} {Virtualno_vrijeme}";
        }

        public List<Object> DohvatiDjecu()
        {
            List<Object> list = new List<Object>();

            Iterator iterator = new Iterator(kolekcijaMolova);

            while (!iterator.IsDone())
            {
                list.Add(iterator.Next());
            }

            return list;
        }

        public List<Vez> DohvatiVezoveMolova()
        {
            List<Vez> vezovi = new List<Vez>();

            Iterator iterator = new Iterator(kolekcijaMolova);

            while (!iterator.IsDone())
            {
                List<Vez> vezoviMola = iterator.Next().DohvatiDjecu().Cast<Vez>().ToList();
                foreach (Vez vez in vezoviMola) vezovi.Add(vez);
            }

            return vezovi;
        }

        public void PopuniKolekcijuVezovaPremaMolovima()
        {
            List<Vez> vezovi = new List<Vez>();
            Iterator iterator = new Iterator(kolekcijaMolova);

            while (!iterator.IsDone())
            {
                List<Vez> vezoviMola = iterator.Next().DohvatiDjecu().Cast<Vez>().ToList();
                int i = 0;
                foreach (Vez vez in vezoviMola)
                {
                    kolekcijaVezova[i] = vez;
                    i++;
                }
            }
        }
    }
}
