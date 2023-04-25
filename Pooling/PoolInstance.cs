using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Gemserk.Utilities.Pooling
{
    [MovedFrom("Gemserk.Utilities")]
    public class PoolInstance<T> : MonoBehaviour where T : class 
    {
        // [NonSerialized]
        // public bool pooled;

        [NonSerialized]
        public T source;
    }
}