using pmihalic_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.IteratorUD
{
    public class Iterator : IIterator
    {
        private Collection collection;
        private int current;
        public Iterator(Collection newCollection)
        {
            this.collection = newCollection;
            current = 0;
        }
        public IBrodskaLukaComponent CurrentItem()
        {
            return collection[current];
        }

        public IBrodskaLukaComponent First()
        {
            return collection[0];
        }

        public bool IsDone()
        {
            return current >= collection.Count;
        }

        public IBrodskaLukaComponent Next()
        {
            if (!IsDone())
            {
                IBrodskaLukaComponent obj = collection[current];
                current++;
                return obj;
            }
            return null;
        }
    }
}
