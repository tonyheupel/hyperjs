using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TonyHeupel.HyperJS
{
    public class JSBoolean : JSObject
    {
        public JSBoolean(dynamic value)
        {
            this.Prototype = JS.cs.Prototype; 
            dynamic that = this;
            that.valueOf = new Func<bool>(delegate() 
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
                });

            that.toString = new Func<string>(() => that.valueOf().ToString().ToLower());
        }
    }
}
