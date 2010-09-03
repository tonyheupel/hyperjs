using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TonyHeupel.HyperJS
{
    // TODO: redefine in terms of something else?
    public class JSNaN : JSObject
    {
        public override string ToString()
        {
            return "NaN";
        }
    }
}
