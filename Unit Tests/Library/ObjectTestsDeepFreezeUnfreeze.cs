using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jurassic;
using Jurassic.Library;

namespace UnitTests
{
    /// <summary>
    /// Test the global Object object.
    /// </summary>
    [TestClass]
    public class ObjectTestsDeepFreezeUnfreeze : TestBase
    {
        [TestMethod]
        public void unfreeze()
        {
            // Simple object
            Assert.AreEqual(true, Evaluate("var x = {a: 1, level2: { a : 1} }; Object.freeze(x) === x"));
            Assert.AreEqual(false, Evaluate("delete x.a"));
            Assert.AreEqual(1, Evaluate("x.a = 2; x.a"));
            Assert.AreEqual(Undefined.Value, Evaluate("x.b = 6; x.b"));
            Assert.AreEqual(PropertyAttributes.Enumerable, EvaluateAccessibility("x", "a"));
            Assert.AreEqual(true, Evaluate("Object.freeze(true).valueOf()"));
            Assert.AreEqual(5, Evaluate("Object.freeze(5).valueOf()"));
            Assert.AreEqual("test", Evaluate("Object.freeze('test').valueOf()"));

            var xObject = (ObjectInstance)ScriptEngine.GetGlobalValue("x");

            xObject.DeepFreeze();
            
            Assert.AreEqual(1, Evaluate("x.level2.a = 2; x.level2.a"));
            Assert.AreEqual(false, Evaluate("delete x.level2.a"));
            Assert.AreEqual(Undefined.Value, Evaluate("x.level2.b = 6; x.level2.b"));
            
            xObject.DeepUnfreeze();
            
            Assert.AreEqual(2, Evaluate("x.a = 2; x.a"));
            Assert.AreEqual(2, Evaluate("x.level2.a = 2; x.level2.a"));
            
            xObject.DeepFreeze();
            
            Assert.AreEqual(2, Evaluate("x.a = 1; x.a"));
            Assert.AreEqual(2, Evaluate("x.level2.a = 1; x.level2.a"));
            
        }
    }
}
