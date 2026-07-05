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
                var configuration = jObject[splitKey.parentKey].Value<IConfiguration>();
                return configuration.Get<T>(splitKey.childKey);
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
                
                var configuration = jObject[splitKey.parentKey].Value<JsonConfiguration>();
                configuration.Set(key, value);
                return;
            }
            jObject[key] = JObject.FromObject(value);
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