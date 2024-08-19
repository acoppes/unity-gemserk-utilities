using System.Linq;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct DamageEffectsComponent : IEntityComponent
    {
        public VfxComponentData[] onDamageEffects;
        public VfxComponentData[] onDeathEffects;
    }
    
    public class DamageEffectsComponentDefinition : ComponentDefinitionBase
    {
        public VfxComponentDataDefinition[] onDamageEffects;
        public VfxComponentDataDefinition[] onDeathEffects;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DamageEffectsComponent()
            {
                onDamageEffects = onDamageEffects.Select(v => v.ToData()).ToArray(),
                onDeathEffects = onDeathEffects.Select(v => v.ToData()).ToArray(),
            });
        }
    }
}