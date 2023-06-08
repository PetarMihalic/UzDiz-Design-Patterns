using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.Visitor
{
    public class ElementVrstaVeza
    {
        virtual public int brojacVezova { get; set; }
        public void Accept(VrstaVezaVisitor vrstaVezaVisitor)
        {
            vrstaVezaVisitor.Visit(this);
        }
    }

    public class VezPO : ElementVrstaVeza
    {
        override public int brojacVezova { get; set; }
    }

    public class VezPU : ElementVrstaVeza
    {
        override public int brojacVezova { get; set; }
    }
    public class VezOS : ElementVrstaVeza
    {
        override public int brojacVezova { get; set; }
    }
}
