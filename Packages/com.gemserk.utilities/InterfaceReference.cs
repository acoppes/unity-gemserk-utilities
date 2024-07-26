using System;

namespace Gemserk.Utilities
{
    [Serializable]
    public class InterfaceReference<T> where T : class
    {
        public UnityEngine.Object source;

        public T Get()
        {
            return source.GetInterface<T>();
        }
    }
}