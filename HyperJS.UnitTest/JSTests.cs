using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TonyHeupel.HyperCore;
using TonyHeupel.HyperJS;

namespace HyperJS.UnitTest
{
    /// <summary>
    /// Unit tests for JS class
    /// </summary>
    [TestClass]
    public class JSTests
    {
 
        [TestMethod]
        public void BooleanFunctionReturnsFalseProperly()
        {
            Assert.IsFalse(JS.cs.Boolean(null));
            Assert.IsFalse(JS.cs.Boolean(0));
            Assert.IsFalse(JS.cs.Boolean(false));
            Assert.IsFalse(JS.cs.Boolean(string.Empty));
            Assert.IsFalse(JS.cs.Boolean("false"));
            Assert.IsFalse(JS.undefined);
            Assert.IsFalse(JS.NaN);

            Assert.IsFalse(JS.cs.NewBoolean(0));
            
        }

        [TestMethod]
        public void BooleanFunctionReturnsTrueProperly()
        {
            Assert.IsTrue(JS.cs.Boolean("False"));
            Assert.IsTrue(JS.cs.Boolean(new object()));
            
            dynamic someThing = JS.cs.NewObject(); //NOTE: May update base class for JS to return undefined instead of throw binding error
            Assert.IsTrue(someThing);
            someThing.foobar = 5;
            Assert.IsTrue(JS.cs.Boolean(someThing.foobar as object));
            Assert.IsTrue(JS.cs.Boolean(" "));

            Assert.IsTrue(JS.cs.NewBoolean(" "));
        }

        [TestMethod]
        public void StringConstructorFunctionReturnsStringProperly()
        {
            dynamic s = JS.cs.NewString("hello");
            Assert.IsInstanceOfType(s, typeof(JSObject));//JSString));
            Assert.AreEqual("String", s.JSTypeName);
            
            Assert.IsInstanceOfType(s.valueOf(), typeof(String));
            Assert.IsInstanceOfType(s.toString(), typeof(String));
            Assert.AreEqual("hello", s.valueOf());
            Assert.AreEqual("hello", s.toString());
        }

        [TestMethod]
        public void PrototypeFun()
        {
            dynamic s = JS.cs.NewString("hello");

            JS.cs.NewObject().Prototype.bizbuzz = new Func<string, string>((name) => "hello, " + name);
            JS.cs.NewString(null).Prototype.foobar = new Func<bool>(() => true);  // Check it doesn't inadvertantly set ALL Prototypes from root object to have foobar

            Assert.IsTrue(s.foobar());

            Assert.AreEqual("hello, tony", s.bizbuzz("tony"));
            dynamic thing = new JSObject();
            Assert.AreEqual("hello, tony", thing.bizbuzz("tony"));

            Assert.AreEqual(3, thing.Count); //foobar should not show up on JSObject's prototype....
            Assert.AreEqual(4, s.Count); // foobar and bizbuzz are on JString's prototype
            
            dynamic newPrototype = new JSObject();
            newPrototype.skunkWorks = "skunky!";
            s.SetPrototype(newPrototype);

            Assert.AreEqual(3, thing.Count); // Updating string's prototype shouldn't mess with Object
            Assert.AreEqual(4, s.Count); // toString, valueOf, skunkWorks, and bizbuzz (no foobar)
            Assert.AreEqual("skunky!", s.skunkWorks); // Can access it through string instance previously created
            Assert.IsFalse(s.foobar);  // Feature detection - since prototype was changed, no longer there!
        }

    }
}
