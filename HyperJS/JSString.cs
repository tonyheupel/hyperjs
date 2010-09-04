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
            this.Prototype = GetPrototype();
            _primitiveValue = (value == null) ? null : (value is JSObject && JS.cs.Boolean(value.toString as string)) ? value.toString() : value.ToString();

            dynamic that = this;

            that.toString = that.valueOf = new Func<string>(() => that.Prototype.toString(that));
        }

        protected override dynamic GetPrototype()
        {
            dynamic p = new JSObject();
            p.toString = p.valueOf = new Func<dynamic, string>((self) => self._primitiveValue);

            return GetPrototype(this.GetType().Name, p);
        }
    }
}
