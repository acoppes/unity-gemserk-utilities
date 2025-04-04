using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct AnimationFromHealthComponent : IEntityComponent
    {
        
    }
    
    public class AnimationFrameFromHealthComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new AnimationFromHealthComponent());
        }
    }
}