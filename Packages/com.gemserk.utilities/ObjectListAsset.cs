using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities
{
    [CreateAssetMenu(menuName = "Gemserk/Object List", fileName = "ObjectListAsset", order = 0)]
    public class ObjectListAsset : ScriptableObject, IObjectList
    {
        public ObjectList objectList = new ObjectList();

        public T FindByName<T>(string name) where T : Object
        {
            return objectList.FindByName<T>(name);
        }
        
        public void CollectByName<T>(string name, List<T> results) where T : Object
        {
            objectList.CollectByName(name, results);
        }
        
        public void Collect<T>(List<T> results) where T : Object
        {
            objectList.Collect(results);
        }
        
        public List<T> Get<T>() where T : Object
        {
            return objectList.Get<T>();
        }
        
        public List<T> GetComponents<T>() where T : Component
        {
            return objectList.GetComponents<T>();
        }
    }
}