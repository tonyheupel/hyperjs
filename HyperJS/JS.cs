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
    /// and dynamic types with my JSObject/HyperHypo (more than C#, less than JavaScript) class.
    /// </summary>
    public class JS : JSObject
    {
        /// <summary>
        /// go is the dynamic instance accessor for the JS
        /// global object.
        /// </summary>
        public static dynamic go { get { return JS.cs; } }

        /// <summary>
        /// Maybe it should be called cs, but I like
        /// the JS.go idea, so fork it an write your own
        /// if you don't like it :-)
        /// </summary>
        public static JS cs 
        {
            get
            {
                lock (syncRoot)
                {
                    if (_globalObject == null)
                    {
                        _globalObject = new JS();
                        //_globalObject.Prototype = new JSObject(); // Single prototype for all
                    }


                }

                return _globalObject;
            }
        }

        private static volatile dynamic _globalObject;
        private static object syncRoot = new Object();

        /// <summary>
        /// Private constructor since the global object is a singleton
        /// </summary>
        private JS() : base(false)
        {
            dynamic that = this;  //Fun JavaScript trick to make "this" a dynamic so we can just bind things to it
            this.Prototypes = new JSObject(false);
        }

        public dynamic Prototypes { get; set; }

        #region undedfined
        private static readonly JSUndefined _undefined = new JSUndefined();
        public static dynamic undefined { get { return _undefined; } }
        #endregion

        #region NaN
        private static readonly JSNaN _nan = new JSNaN();
        public static dynamic NaN { get { return _nan; } }
        #endregion

        #region Infinity
        // TODO: redefine this in terms of Numeric later and reference Numeric.POSITIVE_INFINITY constant?
        //       (actually, I'm hoping Function() or some use of delegate or Func<> returning a dynamic
        //       will allow JSObject to attach properties to method/function instances so I can actually do something
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
        #region Packages - TODO: redefine this in terms of JSObject later?
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
