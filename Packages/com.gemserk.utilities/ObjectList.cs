using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities
{
    public interface IObjectList
    {
        T FindByName<T>(string name) where T : Object;

        void CollectByName<T>(string name, List<T> results) where T : Object;

        void Collect<T>(List<T> results) where T : Object;

        List<T> Get<T>() where T : Object;
    }

    [Serializable]
    public class ObjectList : IObjectList
    {
        [SerializeField]
        [FolderPath]
        private string path;

        // [SerializeField]
        // private ObjectListPath[] paths;

        public List<string> typeFilters = new List<string>();

        [Tooltip("Leave empty to allow all objects")]
        public string pattern = ".*";

        public Regex regex => new Regex(pattern);

        [Tooltip("Used for GetByName methods")]
        public StringComparison nameComparison = StringComparison.OrdinalIgnoreCase;
        
        public List<Object> assets = new List<Object>();

        public string Path
        {
            get => path;
            set => path = value.Replace("\\", "/");
        }

        public string normalizedAssetPath
        {
            get
            {
                var normalizedPath = path.Replace("\\", "/");
                return $"Assets/{normalizedPath}";
            }
        }

        public T FindByName<T>(string name) where T : Object
        {
            return assets.FirstOrDefault(obj => obj.name.Equals(name, nameComparison)) as T;
        }
        
        public void CollectByName<T>(string name, List<T> results) where T : Object
        {
            results.AddRange(assets.OfType<T>().Where(obj => obj.name.Equals(name, nameComparison)));
        }
        
        public void Collect<T>(List<T> results) where T : Object
        {
            results.AddRange(assets.OfType<T>());
        }
        
        public List<T> Get<T>() where T : Object
        {
            var results = new List<T>();
            Collect(results);
            return results;
        }
    }
}