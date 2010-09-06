using System;

namespace TonyHeupel.HyperJS
{
    public static class JSString
    {
        public static string String(this JS js, dynamic value)
        {
            return NewString(js, value).valueOf();
        }

        public static dynamic NewString(this JS js, dynamic value)
        {
            //return new ClassStyle.JSString(value);
            return StringConstructor(js, value);
        }

        private static dynamic StringConstructor(this JS js, dynamic value)
        {
            dynamic s = new JSObject();
            s.JSTypeName = "String";

            // Set up the prototype first
            dynamic p = new JSObject();
            s.Prototype = s.GetPrototype(p);

            // Set up the instance behavior
            var _primitiveValue = (value == null) ? null : (value is JSObject && JS.cs.Boolean(value.toString as string)) ? value.toString() : value.ToString();

            s.toString = s.valueOf = new Func<string>(() => _primitiveValue);

            return s;
        }
    }
}
