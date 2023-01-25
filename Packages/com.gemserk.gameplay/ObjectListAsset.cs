using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Gameplay
{
    [CreateAssetMenu(menuName = "Gemserk/Object List", fileName = "ObjectListAsset", order = 0)]
    public class ObjectListAsset : ScriptableObject, IObjectList
    {
        public ObjectList objectList = new ObjectList();
        
        public string path => objectList.path;

        public string pattern => objectList.pattern;

        public Regex regex => objectList.regex;

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