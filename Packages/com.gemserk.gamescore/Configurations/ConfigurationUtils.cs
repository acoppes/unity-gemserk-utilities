using System;

namespace Game.Configurations
{
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
}