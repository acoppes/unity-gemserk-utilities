using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityReference : MonoBehaviour
    {
        [NonSerialized]
        public Entity entity;
    }
}