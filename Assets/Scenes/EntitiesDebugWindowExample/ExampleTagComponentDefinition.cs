using Gemserk.Leopotam.Ecs;

namespace Scenes.EntitiesDebugWindowExample
{
    public struct ExampleTagComponent : IEntityComponent
    {
        
    }
    
    public class ExampleTagComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ExampleTagComponent());
        }
    }
}