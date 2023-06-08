using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_1.klase
{
    public class Vez
    {
        public int Id { get; set; }
        public string Oznaka_veza { get; set; }
        public string Vrsta { get; set; }
        public int Cijena_veza_po_satu { get; set; }
        public double Maksimalna_duljina { get; set; }
        public double Miksimalana_sirina { get; set; }
        public double Maksimalna_dubina { get; set; }

        public override string ToString()
        {
            return $"{Id} {Oznaka_veza} {Vrsta} {Cijena_veza_po_satu} {Maksimalna_duljina} {Miksimalana_sirina} {Maksimalna_dubina}";
        }
    }
}
