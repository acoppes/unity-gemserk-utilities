using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Object = UnityEngine.Object;

namespace Game.Components
{
    public interface IConfiguration
    {
        bool Has(string key);
        
        bool Get<T>(string key, out T value);

        T Get<T>(string key);

        void Set<T>(string key, T value);

        IConfiguration GetConfiguration(string key);
    }

    public static class ConfigurationUtils
    {
        public static (string parentKey, string childKey) SplitKey(string key)
        {
            if (!key.Contains(".")) 
                return (null, null);
            
            var startIndex = key.IndexOf(".", StringComparison.Ordinal);
                
            var parentKey = key.Substring(0, startIndex);
            var childKey = key.Substring(startIndex + 1);

            return (parentKey, childKey);
        }
    }

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

        public bool Get<T>(string key, out T value)
        {
            if (configurationDict.TryGetValue(key, out var o))
            {
                value = (T) o;
                return true;
            }
            value = default;
            return false;
        }

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
                Get<IConfiguration>(splitKey.parentKey).Set<T>(splitKey.childKey, value);
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
    
    public struct ConfigurationComponent : IEntityComponent
    {
        public IConfiguration configuration;
        public int configuredVersion;
    }
    
    public struct ConfigurationReconfigureComponent : IActionComponent
    {
        
    }
    
    public class ConfigurationComponentDefinition : ComponentDefinitionBase
    { 
        public Object configurationObject;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ConfigurationComponent()
            {
                configuration = configurationObject.GetInterface<IConfiguration>()
            });
        }
    }
}