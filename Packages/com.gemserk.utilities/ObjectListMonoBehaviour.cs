using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities
{
    public class ObjectListMonoBehaviour : MonoBehaviour, IObjectList
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
    }
}