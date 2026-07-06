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
            return jObject[key].Value<T>();
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
            throw new System.NotImplementedException();
        }

        // public IConfiguration GetConfiguration(string key)
        // {
        //     throw new System.NotImplementedException();
        // }
        
        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value);
        }
    }
}