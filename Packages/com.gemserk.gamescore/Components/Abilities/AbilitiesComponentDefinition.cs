using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components.Abilities
{
    public class AbilitiesComponentDefinition : ComponentDefinitionBase
    {
        public List<AbilityDefinitionObject> abilityDefinitions;

        public Transform abilitiesParent;

        public override string GetComponentName()
        {
            return nameof(AbilitiesComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            var abilitiesComponent = AbilitiesComponent.Create();

            foreach (var abilityDefinitionObject in abilityDefinitions)
            {
                abilitiesComponent.Add(abilityDefinitionObject.abilityDefinition.Create());
            }

            if (abilitiesParent != null)
            {
                var abilityDefinitionFromParent = abilitiesParent.GetComponentsInChildren<AbilityDefinitionObject>();
                foreach (var abilityDefinitionObject in abilityDefinitionFromParent)
                {
                    abilitiesComponent.Add(abilityDefinitionObject.abilityDefinition.Create());
                }
            }
            
            world.AddComponent(entity, abilitiesComponent);
        }
    }
}