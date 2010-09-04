using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TonyHeupel.HyperCore;

namespace TonyHeupel.HyperJS
{
    public class JSObject : HyperHypo
    {
        public static implicit operator bool(JSObject o)
        {
            return ConvertToBoolean(o);
        }

        protected static bool ConvertToBoolean(object o)
        {
            return ((dynamic)new JSBoolean(o)).valueOf();
        }

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            //Try using undefined for properties that aren't defined yet and always returning true...
            if (!base.TryGetMember(binder, out result))
            {
                result = JS.undefined;
            }

            return true;
        }

        private object _primitiveValue;


        public JSObject() : this(null, true) {}

        public JSObject(bool createPrototype) : this(null, createPrototype) { }

        public JSObject(dynamic self, bool createPrototype)
        {
            if (self != null & !(self is JSObject)) _primitiveValue = self;

            dynamic that = this;
            self = self ?? that;
            that.toString = new Func<string>(delegate()
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("Number of members: {0}", self.Count));
                foreach (object member in self)
                {
                    sb.AppendLine(member.ToString());
                }

                return sb.ToString();
            });

            that.valueOf = new Func<dynamic>(() => self);

            // Use this class's prototype
            if (createPrototype)
            {
                this.Prototype = GetPrototype("JSObject", new JSObject(false));
            }


            // Set up the JSObject global function (register it if not already registered)
            //if (!JS.cs.Boolean(JS.go.JSObject as object))
            //{
            //    JS.go.JSObject = new Func<dynamic, dynamic>(delegate(dynamic value)
            //    {
            //        return new JSObject(value);
            //    });
            //}
        }

        protected static dynamic GetPrototype(string typeName, dynamic defaultValue)
        {
            string key = String.Format("{0}_prototype", typeName);
            if (!JS.cs.Prototypes.ContainsKey(key))
            {
                JS.cs.Prototypes[key] = defaultValue;
            }

            return JS.cs.Prototypes[key];
        }
    }

}
