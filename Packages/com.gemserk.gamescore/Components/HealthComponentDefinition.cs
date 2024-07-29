using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Game.Components
{
    public class HealthComponentDefinition : ComponentDefinitionBase
    {
        public float health;
        public float invulnerabilityTime;
        public bool autoDestroyOnDeath;
        public bool autoDisableOnDeath;

        public bool hasHealthBar;
        [ConditionalField(nameof(hasHealthBar))] 
        public Vector3 healthBarOffset;
        [ConditionalField(nameof(hasHealthBar))] 
        public int healthBarSize = 1;

        public bool hasDamageNumber;
        [ConditionalField(nameof(hasDamageNumber))] 
        public float damageNumberAccumulator = 0;
        
        public override string GetComponentName()
        {
            return nameof(HealthComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HealthComponent
            {
                total = health,
                current = health,
                damages = new List<DamageData>(),
                processedDamages = new List<DamageData>(),
                temporaryInvulnerability = new Cooldown(invulnerabilityTime),
                autoDestroyOnDeath = autoDestroyOnDeath,
                autoDisableOnDeath = autoDisableOnDeath,
                healEffects = new List<DamageData>()
            });

            if (hasHealthBar)
            {
                world.AddComponent(entity, new HealthBarComponent()
                {
                    offset = healthBarOffset,
                    size = healthBarSize
                });
            }
            
            if (hasDamageNumber)
            {
                world.AddComponent(entity, new HealthDamageNumberComponent()
                {
                    minToShow = damageNumberAccumulator
                });
            }
        }
    }
}