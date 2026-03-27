using Game.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Triggers.Conditions
{
    public class HealthValueProvider : MonoBehaviour, IValueProvider
    {
        public float GetValue(World world, object activator)
        {
            var entity = (Entity)activator;
            return entity.Get<HealthComponent>().current;
        }
    }
}