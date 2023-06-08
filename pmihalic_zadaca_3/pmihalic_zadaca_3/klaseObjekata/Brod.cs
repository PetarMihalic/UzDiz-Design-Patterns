using pmihalic_zadaca_3.klaseObjekata;
using pmihalic_zadaca_3.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pmihalic_zadaca_3.klase
{
    public class Brod : SpojeniBrodoviObserver
    {
        public int Id { get; set; }
        public string Oznaka_broda { get; set; }
        public string Naziv { get; set; }
        public string Vrsta { get; set; }
        public double Duljina { get; set; }
        public double Sirina { get; set; }
        public double Gaz { get; set; }
        public int Maksimalna_brzina { get; set; }
        public int Kapacitet_putnika { get; set; }
        public int Kapacitet_osobnih_vozila { get; set; }
        public int Kapacitet_tereta { get; set; }

        public override string ToString()
        {
            return $"{Id} {Oznaka_broda} {Naziv} {Vrsta} {Duljina} {Sirina} {Gaz} {Maksimalna_brzina} {Kapacitet_putnika} {Kapacitet_osobnih_vozila} {Kapacitet_tereta}";
        }

        public void Update(ObavijestSubject obavijest)
        {
            Console.WriteLine("Poruka za brod " + Id + ": " + obavijest.getPoruka());
        }
    }
}
