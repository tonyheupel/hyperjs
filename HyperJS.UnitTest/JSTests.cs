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
            Assert.IsFalse(JS.cs.Boolean(String.Empty));
            Assert.IsFalse(JS.cs.Boolean("false"));
            Assert.IsFalse(JS.cs.Boolean(JS.undefined));
            Assert.IsFalse(JS.cs.Boolean(JS.NaN));
        }

        [TestMethod]
        public void BooleanFunctionReturnsTrueProperly()
        {
            Assert.IsTrue(JS.cs.Boolean("False"));
            Assert.IsTrue(JS.cs.Boolean(new object()));
            
            dynamic someThing = new HyperHypo(); //NOTE: May update base class for JS to return undefined instead of throw binding error
            Assert.IsTrue(JS.cs.Boolean(someThing));
            someThing.foobar = "foobar";
            Assert.IsTrue(JS.cs.Boolean(someThing.foobar));
            Assert.IsTrue(JS.cs.Boolean(" "));
        }
    }
}
