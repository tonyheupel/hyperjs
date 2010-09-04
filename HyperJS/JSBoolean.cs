using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TonyHeupel.HyperJS
{
    public class JSBoolean : JSObject
    {
        private bool _primitiveValue = false;

        public JSBoolean(dynamic value)
        {
            this.Prototype = GetPrototype();
            _primitiveValue = CalculateBoolean(value);

            dynamic that = this;
          
            that.valueOf = new Func<bool>(() =>  that.Prototype.valueOf(that));

            that.toString = new Func<string>(() => that.Prototype.toString(that));
            
        }

        protected static bool CalculateBoolean(dynamic value)
        {
            dynamic v = (value is JSObject && !(value is JSUndefined || value is JSNaN)) ? value.valueOf() : value;
            if (v == null ||
                (v is String && (v == "" || v == "false")) ||
                (v is Boolean && v == false) ||
                ((v is Int32 || v is Int64 || v is Int16) && v == 0) ||
                v is JSNaN ||
                v is JSUndefined)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected dynamic GetPrototype()
        {
            dynamic p = new JSObject();
            p.valueOf = new Func<dynamic, bool>((self) => self._primitiveValue);
            p.toString = new Func<dynamic, string>((self) => self._primitiveValue ? "true" : "false");

            return GetPrototype("JSBoolean", p) as object;
        }
    }
}
