using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TonyHeupel.HyperJS
{
    public class JSString : JSObject
    {
        private string _primitiveValue;

        public JSString() : this(null) {}

        public JSString(dynamic value)
        {
            this.Prototype = JS.cs.Prototype;

            _primitiveValue = (value == null) ? null : (value is JSObject && JS.cs.Boolean(value.toString as string)) ? value.toString() : value.ToString();

            dynamic that = this;

            that.toString = that.valueOf = new Func<string>(() => _primitiveValue);
        }
    }
}
