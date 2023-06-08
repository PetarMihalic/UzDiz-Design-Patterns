using pmihalic_zadaca_3.klase;
using pmihalic_zadaca_3.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.klaseObjekata
{
    public class Kanal : ObavijestSubject
    {
        public Kanal(string poruka) : base(poruka)
        {
        }

        public int IdKanal { get; set; }
        public int Frekvencija { get; set; }
        public int MaksimalanBroj { get; set; }

        public List<Brod> SpojeniBrodovi { get; set; }
    }
}
