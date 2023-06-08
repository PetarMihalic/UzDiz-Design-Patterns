using pmihalic_zadaca_3.Composite;
using pmihalic_zadaca_3.IteratorUD;
using pmihalic_zadaca_3.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.klaseObjekata
{
    public class Mol : IBrodskaLukaComponent
    {
        public int IdMol { get; set; }
        public string Naziv { get; set; }

        public Collection kolekcijaVezova = new Collection();

        public override string ToString()
        {
            return $"{IdMol} {Naziv}";
        }

        public List<Object> DohvatiDjecu()
        {
            List<Object> list = new List<Object>();

            Iterator iterator = new Iterator(kolekcijaVezova);

            while (!iterator.IsDone())
            {
                list.Add(iterator.Next());
            }
            return list;
        }
    }
}
