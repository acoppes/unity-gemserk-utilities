using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [Serializable]
    public class InterfaceReference<T> where T : class
    {
        [SerializeField]
        private UnityEngine.Object source;

        private T cachedT;

        public UnityEngine.Object Source => source;
        
        public T Get()
        {
            if (!source)
            {
                return null;
            }

            if (cachedT == null)
            {
                cachedT = source.GetInterface<T>();
            }
            
            return cachedT;
        }
    }
}