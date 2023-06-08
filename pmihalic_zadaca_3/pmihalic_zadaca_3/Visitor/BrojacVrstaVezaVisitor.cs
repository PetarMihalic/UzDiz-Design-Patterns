using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.Visitor
{
    internal class BrojacVrstaVezaVisitor : VrstaVezaVisitor
    {
        public void Visit(ElementVrstaVeza elementVrstaVeza)
        {
            elementVrstaVeza.brojacVezova += 1;
        }
    }
}
