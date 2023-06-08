using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.MementoUD
{
    public class Caretaker
    {
        private List<Memento> mementos = new List<Memento>();

        public void spremi(Memento memento)
        {
            mementos.Add(memento);
        }
        public Memento dohvati(string naziv)
        {
            foreach (Memento memento in mementos)
            {
                if (memento.Naziv == naziv)
                {
                    return memento;
                }
            }
            return null;
        }
    }
}
