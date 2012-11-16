using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using TonyHeupel.HyperCore;
using TonyHeupel.HyperJS;

namespace HyperJS.UnitTest
{
    /// <summary>
    /// Unit tests for JS class
    /// </summary>
    [TestFixture]
    public class JSTests
    {
 
        [Test]
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

        [Test]
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

        [Test]
        public void StringConstructorFunctionReturnsStringProperly()
        {
            dynamic s = JS.cs.NewString("hello");
			Assert.IsInstanceOfType(typeof(JSObject), s);//JSString));
            Assert.AreEqual("String", s.JSTypeName);
            
			Assert.IsInstanceOfType(typeof(String), s.valueOf());
			Assert.IsInstanceOfType(typeof(String), s.toString());
            Assert.AreEqual("hello", s.valueOf());
            Assert.AreEqual("hello", s.toString());
        }

        [Test]
        public void PrototypeFun()
        {
            dynamic s = JS.cs.NewString("hello");
            dynamic thing = new JSObject();

            thing.Prototype.bizbuzz = new Func<string, string>((name) => "hello, " + name);
            s.Prototype.foobar = new Func<bool>(() => true);  // Check it doesn't inadvertantly set ALL Prototypes from root object to have foobar

            // bizbuzz method available on all objects
            Assert.AreEqual("hello, tony", s.bizbuzz("tony"));
            Assert.AreEqual("hello, tony", thing.bizbuzz("tony"));

            // foobar set oon string prototype, but not exposed to object
            Assert.IsTrue(s.foobar()); 
            Assert.IsFalse(thing.foobar); // Feature detection -- not set on object prototype


            Assert.AreEqual(3, thing.Count); //foobar should not show up on JSObject's prototype....
            Assert.AreEqual(4, s.Count); // foobar and bizbuzz are available to previously created string instances
            
            // Create new string prototype and set it
            dynamic newPrototype = new JSObject();
            newPrototype.skunkWorks = "skunky!";
            s.SetPrototype(newPrototype);  // skunkWorks now available on string

            Assert.AreEqual(3, thing.Count);  // Updating string's prototype shouldn't mess with Object
            Assert.IsFalse(thing.skunkWorks); // Feature detection - make sure skunkWorks not on object
            Assert.AreEqual(4, s.Count); // toString, valueOf, skunkWorks, and bizbuzz (no foobar)
            Assert.AreEqual("skunky!", s.skunkWorks); // Can access it through string instance previously created
            Assert.IsFalse(s.foobar);  // Feature detection - since prototype was changed, no longer there!
        }

    }
}
