using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    #if GEMSERK_DEBUG_ENTITYREFERENCE && UNITY_EDITOR
    public class EntityReference : MonoBehaviour
    {
        [NonSerialized]
        public Entity entity;
    }
    #endif
}