using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.klase
{
    public class Raspored
    {
        public int Id_vez { get; set; }
        public int Id_brod { get; set; }
        public string Dani_u_tjednu { get; set; }
        public TimeOnly Vrijeme_od { get; set; }
        public TimeOnly Vrijeme_do { get; set; }

        public override string ToString()
        {
            return $"{Id_vez} {Id_brod} {Dani_u_tjednu} {Vrijeme_od} {Vrijeme_do}";
        }
    }
}
