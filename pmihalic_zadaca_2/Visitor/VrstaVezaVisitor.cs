﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.Visitor
{
    public interface VrstaVezaVisitor
    {
        public void Visit(ElementVrstaVeza elementVrstaVeza);
    }
}
