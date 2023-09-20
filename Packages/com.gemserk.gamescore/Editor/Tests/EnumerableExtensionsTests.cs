using System.Collections.Generic;
using NUnit.Framework;

namespace Game.Editor.Tests
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void Test_ContainsAll()
        {
            var list1 = new List<string>()
            {
                "a", "b", "c"
            };

            var list2 = new List<string>()
            {
                "b"
            };

            var list3 = new List<string>();
            
            Assert.IsTrue(list1.ContainsAll(list2));
            Assert.IsTrue(list1.ContainsAll(list3));
            Assert.IsFalse(list2.ContainsAll(list1));
            
            Assert.IsTrue(list1.ContainsAll(list1));
        }
    
        [Test]
        public void Test_ContainsNone()
        {
            var list1 = new List<string>()
            {
                "a", "b", "c"
            };

            var list2 = new List<string>()
            {
                "b"
            };
            
            var list3 = new List<string>()
            {
                "d"
            };
            
            var list4 = new List<string>();
            
            Assert.IsFalse(list1.ContainsNone(list2));
            Assert.IsTrue(list1.ContainsNone(list3));
            Assert.IsTrue(list1.ContainsNone(list4));
        }
    }
}