using pmihalic_zadaca_2.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.ChainOfResponsibility
{
    public interface LanacRaspodjelaPutnika
    {
        void setNextChain(LanacRaspodjelaPutnika lanacRaspodjelaPutnika);
        
        void raspodjeli(Putnici putnici, List<Brod> brodovi);
    }
}
