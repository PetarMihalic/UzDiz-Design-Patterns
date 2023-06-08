using pmihalic_zadaca_3.IteratorUD;
using pmihalic_zadaca_3.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.MementoUD
{
    public class Memento
    {
        string naziv;
        Collection vezovi;
        DateTime vrijeme;
        public Memento(string naziv, Collection vezovi, DateTime vrijeme)
        {
            this.naziv = naziv;
            this.vezovi = vezovi;
            this.vrijeme = vrijeme;
        }
        public DateTime Vrijeme
        {
            get { return vrijeme; }
        }
        public Collection Vezovi
        {
            get { return vezovi; }
        }
        public string Naziv
        {
            get { return naziv; }
        }
    }
}
