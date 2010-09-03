using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TonyHeupel.HyperCore;

namespace TonyHeupel.HyperJS
{
    public class JSObject : HyperHypo
    {
        public JSObject() : this (null)
        {

        }
        public JSObject(JSObject self)
        {
            if (JS.cs != null) this.Prototype = JS.cs.Prototype;

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
        }
        public static implicit operator bool(JSObject o)
        {
            return ConvertToBoolean(o);
        }

        protected static bool ConvertToBoolean(object o)
        {
            return JS.go.Boolean(o).valueOf();
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
    }
}
