using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;

namespace Game.Components
{
    public struct DecalComponent : IEntityComponent
    {
        public Cooldown ttl;
        public IntAccumulator phases;
    }
    
    public class DecalComponentDefinition : ComponentDefinitionBase
    {
        public float ttl;
        public int phases;
        
        public override string GetComponentName()
        {
            return nameof(DecalComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DecalComponent()
            {
                ttl = new Cooldown(ttl),
                phases = new IntAccumulator()
                {
                    total = phases,
                    current = phases
                }
            });
        }
    }
}