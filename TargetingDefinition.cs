using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class TargetingDefinition : MonoBehaviour, IEntityDefinition
    {
        public TargetingParameters targetingParameters;
    
        public void Apply(World world, int entity)
        {
            ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
            abilitiesComponent.targetings.Add(new Targeting
            {
                name = gameObject.name,
                parameters = targetingParameters
            });
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, targetingParameters.range);
        }
    }
}