using Game.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Triggers.Conditions
{
    public class HealthValueProvider : MonoBehaviour, IValueProvider
    {
        public bool useTotal;
        
        public float GetValue(World world, object activator)
        {
            var entity = (Entity)activator;
            if (useTotal)
            {
                return entity.Get<HealthComponent>().total;
            }
            return entity.Get<HealthComponent>().current;
        }
    }
}