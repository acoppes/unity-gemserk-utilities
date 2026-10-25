using NUnit.Framework;
using UnityEngine;

namespace Gemserk.Utilities.Tests
{
    public class ObjectListTests
    {
        private class ComponentForTest : MonoBehaviour
        {
            
        }
        
        [Test]
        public void Test_ObjectList_GetCorrespondingObject()
        {
            var objectList = new ObjectList();
            var gameObject1 = new GameObject("object1");
            var gameObject2 = new GameObject("object2");
            
            objectList.assets.Add(gameObject1);
            objectList.assets.Add(gameObject2);

            Assert.IsNull(objectList.FindByName<GameObject>("not_object"));
            Assert.AreSame(gameObject1, objectList.FindByName<GameObject>("object1"));
            Assert.AreSame(gameObject2, objectList.FindByName<GameObject>("object2"));
        }
        
        [Test]
        public void Test_ObjectList_GetComponentFromObject()
        {
            var objectList = new ObjectList();
            var gameObject1 = new GameObject("object1");
            var c1 = gameObject1.AddComponent<ComponentForTest>();
            objectList.assets.Add(gameObject1);
            Assert.AreSame(c1, objectList.FindComponentByName<ComponentForTest>("object1"));
        }
    }
}