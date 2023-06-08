using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.klaseObjekata
{
    public class Mol
    {
        public int IdMol { get; set; }
        public string Naziv { get; set; }

        public override string ToString()
        {
            return $"{IdMol} {Naziv}";
        }
    }
}
