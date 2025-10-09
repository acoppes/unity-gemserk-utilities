using System.Linq;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Serialization;

namespace Game.Components
{
    public struct DamageEffectsComponent : IEntityComponent
    {
        public SpawnData[] onDamageSpawns;
        public SpawnData[] onDeathSpawns;
    }
    
    public class DamageEffectsComponentDefinition : ComponentDefinitionBase
    {
        [FormerlySerializedAs("onDamageEffects")] 
        public SpawnDataDefinition[] onDamageSpawns;
        [FormerlySerializedAs("onDeathEffects")] 
        public SpawnDataDefinition[] onDeathSpawns;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DamageEffectsComponent()
            {
                onDamageSpawns = onDamageSpawns.Select(v => v.ToData()).ToArray(),
                onDeathSpawns = onDeathSpawns.Select(v => v.ToData()).ToArray(),
            });
        }
    }
}