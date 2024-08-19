using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct DestroyOnAnimationCompleteComponent : IEntityComponent
    {
        
    }
    
    public class DestroyOnAnimationCompleteComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyOnAnimationCompleteComponent());
        }
    }
}