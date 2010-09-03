﻿using System;
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
            
            dynamic someThing = JS.cs.Object(); //NOTE: May update base class for JS to return undefined instead of throw binding error
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
            Assert.IsInstanceOfType(s, typeof(JSString));
            // Need to fix the Prototype such that the functions themselves have properties too...
            s.Prototype.foobar = new Func<bool>(() => true);
            Assert.IsInstanceOfType(s.valueOf(), typeof(String));
            Assert.IsInstanceOfType(s.toString(), typeof(String));
            Assert.AreEqual("hello", s.valueOf());
            Assert.AreEqual("hello", s.toString());
            Assert.IsTrue(s.foobar());
        }

    }
}
