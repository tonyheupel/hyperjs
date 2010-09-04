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
                // Note: NEED to use "JSObject" as hard-coded string here or every constructor
                // for any base classes will have an inadvertant prototype created since
                // this.GetType().Name will return the base class name...that should be 
                // handled later in the sublcass constructor with an explicit line.
                this.Prototype = GetPrototype("JSObject", new JSObject(false));
                JS.cs.Prototypes.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnPrototypeChanged);
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

        protected void OnPrototypeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPrototypeKeyName(this.GetType().Name)) this.Prototype = GetPrototype();
        }

        protected virtual dynamic GetPrototype()
        {
            return GetPrototype(this.GetType().Name);
        }

        protected static dynamic GetPrototype(string typeName)
        {
            return GetPrototype(typeName, null);
        }
        protected static dynamic GetPrototype(string typeName, dynamic defaultValue)
        {
            string key = GetPrototypeKeyName(typeName);
            if (!JS.cs.Prototypes.ContainsKey(key) && defaultValue != null)
            {
                SetPrototype(typeName, defaultValue);
            }

            return JS.cs.Prototypes[key];
        }

        protected static void SetPrototype(string typeName, dynamic value)
        {
            JS.cs.Prototypes[GetPrototypeKeyName(typeName)] = value;
        }
        private static string GetPrototypeKeyName(string typeName)
        {
            return String.Format("{0}_prototype", typeName);
        }
    }

}
