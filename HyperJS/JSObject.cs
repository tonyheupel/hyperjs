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
            //TODO: See if we can remove the tight coupling with JS.cs here...
            return JS.cs.Boolean(o);
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

        public string JSTypeName { get; set; }

        public JSObject() : this(null, true) {}

        public JSObject(bool createPrototype) : this(null, createPrototype) { }

        public JSObject(dynamic self, bool createPrototype)
        {
            this.JSTypeName = this.GetType().Name; // Default to old-school class inheritance

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
                // for any subclasses will have an inadvertant prototype created since
                // JSTypeName will return the base class name...that should be 
                // handled later in the sublcass constructor with an explicit line.
                this.Prototype = GetPrototype("JSObject", new JSObject(false));
                JS.cs.Prototypes.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnPrototypeChanged);
            }
        }

        protected void OnPrototypeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPrototypeKeyName(JSTypeName)) this.Prototype = GetPrototype();
        }

        public virtual dynamic GetPrototype()
        {
            return GetPrototype(JSTypeName);
        }
        public virtual dynamic GetPrototype(JSObject defaultValue)
        {
            return GetPrototype(JSTypeName, defaultValue);
        }
        public static dynamic GetPrototype(string typeName)
        {
            return GetPrototype(typeName, null);
        }
        public static dynamic GetPrototype(string typeName, dynamic defaultValue)
        {
            string key = GetPrototypeKeyName(typeName);
            if (!JS.cs.Prototypes.ContainsKey(key) && defaultValue != null)
            {
                SetPrototype(typeName, defaultValue);
            }

            return JS.cs.Prototypes[key];
        }

        public static void SetPrototype(string typeName, dynamic value)
        {
            JS.cs.Prototypes[GetPrototypeKeyName(typeName)] = value;
        }
        protected static string GetPrototypeKeyName(string typeName)
        {
            return String.Format("{0}_prototype", typeName);
        }
    }

}
