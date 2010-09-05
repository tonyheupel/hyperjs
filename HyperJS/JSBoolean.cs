using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TonyHeupel.HyperJS
{
    public static class JSBoolean
    {
        /// <summary>
        /// The Boolean(value) function on the global object.
        /// It returns a Boolean converted from the value passed in. 
        /// 0, NaN, null, "", undefined, false and "false" are false.
        /// Everything else will return true.
        /// </summary>
        public static bool Boolean(this JS js, object value)
        {
            return NewBoolean(js, value).valueOf();
        }

        public static dynamic NewBoolean(this JS js, dynamic value)
        {
            //return new ClassStyle.JSBoolean(value);
            return BooleanConstructor(js, value);
        }

        public static dynamic BooleanConstructor(this JS js, dynamic value)
        {
            dynamic b = new JSObject();
            b.JSTypeName = "Boolean";

            // Set up prototype
            dynamic p = new JSObject();
            b.Prototype = b.GetPrototype(p);

            // Calculate the primitive value
            bool _primitiveValue = true; //default to true and only set to false when needed
            dynamic v = (value is JSObject && !(value is JSUndefined || value is JSNaN)) ? value.valueOf() : value;
            if (v == null ||
                (v is String && (v == "" || v == "false")) ||
                (v is Boolean && v == false) ||
                ((v is Int32 || v is Int64 || v is Int16) && v == 0) ||
                v is JSNaN ||
                v is JSUndefined)
            {
                _primitiveValue = false;
            }


            // Set up instance items
            b.valueOf = new Func<bool>(() => _primitiveValue);
            b.toString = new Func<string>(() => _primitiveValue ? "true" : "false"); //Consider using String() here

            return b;
        }
    }
}
