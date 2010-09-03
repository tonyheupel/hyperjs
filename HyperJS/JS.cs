using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TonyHeupel.HyperCore;

namespace TonyHeupel.HyperJS
{
    /// <summary>
    /// The Global object.
    /// JS.go - Playing with JavaScript-type object creation within C# using closures
    /// and dynamic types with my HyperJS/HyperHypo (more than C#, less than JavaScript) class.
    /// </summary>
    public class JS : JSObject
    {
        /// <summary>
        /// go is the dynamic instance accessor for the JS
        /// global object.
        /// </summary>
        public static dynamic go { get { return _globalObject; } }

        /// <summary>
        /// Maybe it should be called cs, but I like
        /// the JS.go idea, so fork it an write your own
        /// if you don't like it :-)
        /// </summary>
        public static JS cs { get { return _globalObject; } }
        // TODO: Determine if this actually works for the singleton initialization...
        //       Normally, I would put it on the getter and do a first-time-accessed
        //       initialization with a lock for thread safety, but I think that would
        //       be slower than having it load when this comes into the app domain
        //       initially (but I'm not yet sure if that's what's really happening
        //       or if it really is thread-safe yet)...
        private static dynamic _globalObject = new JS();
        
        /// <summary>
        /// Private constructor since the global object is a singleton
        /// </summary>
        private JS()
        {
            this.Prototype = new JSObject();
            dynamic that = this;  //Fun JavaScript trick to make "this" a dynamic so we can just bind things to it

            /// <summary>
            /// The Object() function on the global object.  It returns a new instance of
            /// a dynamic object.  Pass in a non-null self parameter when doing prototype
            /// inheritance so that Object's methods have access to the real "this" (since
            /// this won't work the same way as in JavaScript.)
            /// </summary>
            that.Object = new Func<dynamic, dynamic>(delegate(dynamic self)
            {
                dynamic o = new JSObject();
                self = self ?? o;
                o.toString = new Func<string>(delegate() 
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine(string.Format("Number of members: {0}", self.Count));
                        foreach (object member in self)
                        {
                            sb.AppendLine(member.ToString());
                        }

                        return sb.ToString();
                    });

                o.valueOf = new Func<dynamic>(() => self);

                return o;
            });

            that.Boolean = new Func<dynamic, dynamic>(delegate(dynamic value)
                {
                    dynamic b = new JSObject();

                    b.valueOf = new Func<bool>(delegate() 
                        {
                            dynamic v = (value is JSObject && !(value is Undefined || value is NaNClass)) ? value.valueOf() : value;
                            if (v == null ||
                               (v is String && (v == "" || v == "false")) ||
                               (v is Boolean && v == false) ||
                               ((v is Int32 || v is Int64 || v is Int16) && v == 0) ||
                               v is NaNClass ||
                               v is Undefined)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        });

                    b.toString = new Func<string>(() => b.valueOf().ToString().ToLower());

                    return b;
                });

            that.String = new Func<dynamic, dynamic>(delegate(dynamic value)
                {
                    dynamic s = new JSObject();
                    s.Prototype = that.Object(s);

                    string _primitiveStringValue = (value == null) ? null : (value is JSObject && that.Boolean(value.toString)) ? value.toString() : value.ToString();

                    s.valueOf = s.toString = new Func<string>(() => _primitiveStringValue);

                    return s;
                });
        }

        #region undedfined
        private class Undefined : JSObject
        {
            public override string ToString()
            {
                return "undefined";
            }
        }
        private static readonly Undefined _undefined = new Undefined();
        public static dynamic undefined { get { return _undefined; } }
        #endregion

        #region NaN
        // TODO: redefine in terms of something else?
        private class NaNClass : JSObject
        {
            public override string ToString()
            {
                return "NaN";
            }
        }
        private static readonly NaNClass _nan = new NaNClass();
        public static dynamic NaN { get { return _nan; } }
        #endregion

        #region Infinity
        // TODO: redefine this in terms of Numeric later and reference Numeric.POSITIVE_INFINITY constant?
        //       (actually, I'm hoping Function() or some use of delegate or Func<> returning a dynamic
        //       will allow HyperJS to attach properties to method/function instances so I can actually do something
        //       like Numeric.POSITIVE_INFINITY; otherwise, I'll have to fake it with a Numeric class and a static
        //       public constant; then creating a Numeric is "JS.Numeric(value)" and there will be
        //       "Numeric.POSITIVE_INFINITY" on a Numeric class...trying to avoid creating actual C# classes, though
        //       since the experiment is to use functions and closures like JavaScript.
        private class InfinityClass : JSObject
        {
            public override string ToString()
            {
                return "Infinity";
            }
        }
        private static readonly InfinityClass _infinity = new InfinityClass();
        public static dynamic Infinity { get { return _infinity; } }
        #endregion

        #region junk - Java-related items
        #region Packages - TODO: redefine this in terms of Object later?
        private class JavaPackage : JSObject
        {
            string Name { get; set; }
            private List<JavaPackage> _children = new List<JavaPackage>();
            IEnumerable<JavaPackage> Packages
            {
                get
                {
                    return _children;
                }
            }
            //TODO: Figure out what to do here--probably nothing
            public override string ToString()
            {
                return "JavaPackage";
            }
        }
        private static readonly JavaPackage _packages = new JavaPackage();
        public static dynamic Packages { get { return _packages; } }

        public static dynamic java { get { return _packages; } } //TODO: Fix this so there is a java.* package and that this returns that
        #endregion
        #endregion

        public class Math : JSObject
        {

        }
    }
}
