using pmihalic_zadaca_3.Composite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.IteratorUD
{
    public class Collection : ICollection
    {
        private ArrayList items = new ArrayList();
        public Iterator CreateIterator()
        {
            return new Iterator(this);
        }

        public int Count
        {
            get{
                return items.Count;
            }
        }

        public IBrodskaLukaComponent this[int index]
        {
            get
            {
                return (IBrodskaLukaComponent)items[index];
            }
            set
            {
                items.Add(value);
            }
        }
    }
}
