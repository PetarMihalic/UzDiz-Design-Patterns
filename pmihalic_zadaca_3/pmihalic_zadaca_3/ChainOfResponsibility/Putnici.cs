using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.ChainOfResponsibility
{
    public class Putnici
    {
        private int brojPutnika;

        public Putnici(int brojPutnika)
        {
            this.brojPutnika = brojPutnika;
        }

        public int getBrojPutnika() 
        { 
            return this.brojPutnika; 
        }
    }
}
