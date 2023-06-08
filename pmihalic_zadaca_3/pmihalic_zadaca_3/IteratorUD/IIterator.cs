using pmihalic_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.IteratorUD
{
    public interface IIterator
    {
        IBrodskaLukaComponent First();
        IBrodskaLukaComponent Next();
        bool IsDone();
        IBrodskaLukaComponent CurrentItem();
    }
}
