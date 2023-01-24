using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Gameplay
{
    [CreateAssetMenu(menuName = "Gemserk/Object List", fileName = "ObjectListAsset", order = 0)]
    public class ObjectListAsset : ScriptableObject
    {
        public string path;

        [Tooltip("Leave empty to allow all objects")]
        public string pattern = ".*";

        public Regex regex => new Regex(pattern);

        [Tooltip("Used for GetByName methods")]
        public StringComparison defaultComparison = StringComparison.OrdinalIgnoreCase;
        
        public List<Object> assets = new List<Object>();

        public T GetByName<T>(string name) where T : Object
        {
            return assets.FirstOrDefault(obj => obj.name.Equals(name, defaultComparison)) as T;
        }
        
        public void GetByName<T>(string name, List<T> results) where T : Object
        {
            results.AddRange(assets.Where(obj => obj.name.Equals(name, defaultComparison))
                .Cast<T>());
        }
    }
}