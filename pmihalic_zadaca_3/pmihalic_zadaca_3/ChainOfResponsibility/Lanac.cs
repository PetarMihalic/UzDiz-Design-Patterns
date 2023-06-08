using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.ChainOfResponsibility
{
    public class Lanac
    {
        public LanacRaspodjelaPutnika lanac1;

        public Lanac()
        {
            this.lanac1 = new RaspodjelaBrod1();
            LanacRaspodjelaPutnika lanac2 = new RaspodjelaBrod2();
            LanacRaspodjelaPutnika lanac3 = new RaspodjelaBrod3();
            LanacRaspodjelaPutnika lanac4 = new RaspodjelaBrod4();
            LanacRaspodjelaPutnika lanac5 = new RaspodjelaBrod5();

            lanac1.setNextChain(lanac2);
            lanac2.setNextChain(lanac3);
            lanac3.setNextChain(lanac4);
            lanac4.setNextChain(lanac5);
        }
    }
}
