using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_1.klase
{
    public class Luka
    {
        public string Naziv { get; set; }
        public double GPS_sirina { get; set; }
        public double GPS_visina { get; set; }
        public int Dubina_luke { get; set; }
        public int Ukupni_broj_putnickih_vezova { get; set; }
        public int Ukupni_broj_poslovnih_vezova { get; set; }
        public int Ukupni_broj_ostalih_vezova { get; set; }
        public DateTime Virtualno_vrijeme { get; set; }

        public override string ToString()
        {
            return $"{Naziv} {GPS_sirina} {GPS_visina} {Dubina_luke} {Ukupni_broj_putnickih_vezova} {Ukupni_broj_poslovnih_vezova} {Ukupni_broj_ostalih_vezova} {Virtualno_vrijeme}";
        }
    }
}
