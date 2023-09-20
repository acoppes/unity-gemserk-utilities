using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct ParallaxComponent : IEntityComponent
    {
        public float speed;
        // public float depth;
    }
    
    public class ParallaxComponentDefinition : ComponentDefinitionBase
    {
        public float speed = 1;
        // public float depth = 1;
        
        public override string GetComponentName()
        {
            return nameof(ParallaxComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ParallaxComponent()
            {
                speed = speed,
                // depth = depth
            });
        }
    }
}