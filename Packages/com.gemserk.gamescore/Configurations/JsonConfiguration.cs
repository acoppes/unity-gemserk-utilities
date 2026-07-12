using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Game.Configurations
{
    public class JsonConfiguration : IConfiguration
    {
        private JObject jObject;

        public JsonConfiguration()
        {
            jObject = new JObject();
        }
        
        public JsonConfiguration(JObject jObject)
        {
            this.jObject = jObject;
        }


        public bool Has(string key)
        {
            return jObject.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);
            if (splitKey.parentKey != null)
            {
                var configurationObject = jObject[splitKey.parentKey] as JObject;
                return new JsonConfiguration(configurationObject).Get<T>(splitKey.childKey);
            }
            return jObject[key].ToObject<T>();
        }

        public void Set<T>(string key, T value)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);
            if (splitKey.parentKey != null)
            {
                if (!jObject.ContainsKey(splitKey.parentKey))
                {
                    jObject[splitKey.parentKey] = new JObject();
                }

                var childConfiguration = jObject[splitKey.parentKey] as JObject;
                var childJsonConfiguration = new JsonConfiguration(childConfiguration);
                childJsonConfiguration.Set(splitKey.childKey, value);
                
                // childConfiguration.Add(splitKey.childKey, JToken.FromObject(value));
                
                return;
            }
            
            jObject.Add(key, JToken.FromObject(value));
        }

        public IConfiguration GetConfiguration(string key)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);
            if (splitKey.parentKey != null)
            {
                var childConfiguration = jObject[splitKey.parentKey] as JObject;
                if (childConfiguration != null)
                {
                    return new JsonConfiguration(childConfiguration).GetConfiguration(splitKey.childKey);
                }
                
                return null;
            }
            
            var subConfiguration = jObject[key] as JObject;
            if (subConfiguration != null)
            {
                return new JsonConfiguration(subConfiguration);
            }
            
            return null;
        }

        public IConfiguration[] GetConfigurationArray(string key)
        {
            var list = new List<IConfiguration>();
            var jArray = jObject[key] as JArray;

            foreach (var child in jArray)
            {
                list.Add(new JsonConfiguration(child as JObject));
            }

            return list.ToArray();
        }
        
        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value);
        }
    }
}