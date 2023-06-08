using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.klaseObjekata
{
    public class MolVez
    {
        public int IdMol { get; set; }
        public List<int> IdVezovi { get; set; }

        public override string ToString()
        {
            return $"{IdMol} {IdVezovi}";
        }
    }
}
