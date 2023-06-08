using System;

namespace Gemserk.Utilities
{
    [Serializable]
    public class SelectablePath
    {
        public string path;
        
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
    }
}