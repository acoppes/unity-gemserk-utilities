using NUnit.Framework;
using UnityEngine;

namespace Gemserk.Utilities.Tests
{
    public static class AssertExtensions
    {
        public static void AreEqual(Vector2 expected, Vector2 actual, double delta)
        {
            Assert.AreEqual(expected.x, actual.x, delta, "x");
            Assert.AreEqual(expected.y, actual.y, delta, "y");
        }
    }
    
    public class VectorExtensionTests
    {
        [Test]
        public void Test_FixToAngles()
        {
            AssertExtensions.AreEqual(new Vector2(1, 0), new Vector2(1, 1).FixToAngles(180), 0.01f);
            AssertExtensions.AreEqual(new Vector2(-1, 0), new Vector2(-1, 1).FixToAngles(180), 0.01f);
            
            AssertExtensions.AreEqual(new Vector2(0, 1), new Vector2(0, 1).FixToAngles(45), 0.01f);
            AssertExtensions.AreEqual(new Vector2(1, 0), new Vector2(1, 0).FixToAngles(45), 0.01f);
            AssertExtensions.AreEqual(new Vector2(-1, 0), new Vector2(-1, 0).FixToAngles(45), 0.01f);
            AssertExtensions.AreEqual(new Vector2(0, -1), new Vector2(0, -1).FixToAngles(45), 0.01f);
        }
    }
}