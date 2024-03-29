﻿using System;
using UnityEngine;

namespace Gemserk.Utilities.Pooling
{
    public class PoolInstance<T> : MonoBehaviour where T : class 
    {
        // [NonSerialized]
        // public bool pooled;

        [NonSerialized]
        public T source;
    }
}