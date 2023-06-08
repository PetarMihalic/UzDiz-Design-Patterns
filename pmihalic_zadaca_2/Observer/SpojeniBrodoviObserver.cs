using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.Observer
{
    public interface SpojeniBrodoviObserver
    {
        void Update(ObavijestSubject obavijest);
    }
}
