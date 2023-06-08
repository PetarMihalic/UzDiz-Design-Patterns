using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.Observer
{
    public abstract class ObavijestSubject
    {
        private string poruka { get; set; }
        private List<SpojeniBrodoviObserver> brodovi = new List<SpojeniBrodoviObserver>();

        protected ObavijestSubject(string poruka)
        {
            this.poruka = poruka;
        }

        public void Attach(SpojeniBrodoviObserver brod)
        {
            brodovi.Add(brod);
        }
        public void Detach(SpojeniBrodoviObserver brod)
        {
            brodovi.Remove(brod);
        }
        public void Notify()
        {
            foreach (SpojeniBrodoviObserver brod in brodovi)
            {
                brod.Update(this);
            }
            Console.WriteLine("");
        }
        public ObavijestSubject setPoruka(String novaPoruka)
        {
            this.poruka = novaPoruka;
            Notify();
            return this;
        }
        public string getPoruka()
        {
            return this.poruka;
        }
    }
}
