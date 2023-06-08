using pmihalic_zadaca_3.IteratorUD;
using pmihalic_zadaca_3.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.MementoUD
{
    public class Originator
    {
        Collection vezovi;
        DateTime vrijeme;

        public Collection Vezovi
        {
            get { return vezovi; }
            set { vezovi = value; }
        }

        public DateTime Vrijeme
        {
            get { return vrijeme; }
            set { vrijeme = value; }
        }
        public Memento CreateMemento(string naziv)
        {
            return (new Memento(naziv, vezovi, vrijeme));
        }
        public void SetMemento(Memento memento)
        {
            vezovi = memento.Vezovi;
            vrijeme = memento.Vrijeme;
        }
    }
}
