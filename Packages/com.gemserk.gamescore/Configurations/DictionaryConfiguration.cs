using System.Collections.Generic;

namespace Game.Configurations
{
    public class DictionaryConfiguration : IConfiguration
    {
        private Dictionary<string, object> configurationDict;

        public DictionaryConfiguration()
        {
            configurationDict = new Dictionary<string, object>();
        }
        
        public DictionaryConfiguration(Dictionary<string, object> configurationDict)
        {
            this.configurationDict = configurationDict;
        }

        public bool Has(string key)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);
            if (splitKey.parentKey != null)
            {
                return Get<IConfiguration>(splitKey.parentKey).Has(splitKey.childKey);
            }
            return configurationDict.ContainsKey(key);
        }

        // public bool Get<T>(string key, out T value)
        // {
        //     if (configurationDict.TryGetValue(key, out var o))
        //     {
        //         value = (T) o;
        //         return true;
        //     }
        //     value = default;
        //     return false;
        // }

        public T Get<T>(string key)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);

            if (splitKey.parentKey != null)
            {
                return Get<IConfiguration>(splitKey.parentKey).Get<T>(splitKey.childKey);
            }
            
            return (T) configurationDict[key];
        }
        
        public void Set<T>(string key, T value)
        {
            var splitKey = ConfigurationUtils.SplitKey(key);

            if (splitKey.parentKey != null)
            {
                if (!configurationDict.ContainsKey(splitKey.parentKey))
                {
                    configurationDict[splitKey.parentKey] = new DictionaryConfiguration();
                }
                
                Get<IConfiguration>(splitKey.parentKey).Set(splitKey.childKey, value);
                return;
            }
            
            configurationDict[key] = value;
        }

        public IConfiguration GetConfiguration(string key)
        {
            if (configurationDict.TryGetValue(key, out var o))
            {
                if (o is IConfiguration configuration)
                {
                    return configuration;
                }
            }
            return null;
        }
        
        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value);
        }
    }
}