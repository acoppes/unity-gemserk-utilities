namespace Gemserk.Leopotam.Ecs.Components
{
    public struct StaticObjectComponent : IEntityComponent
    {
        
    }
    
    public class StaticObjectComponentDefinition : ComponentDefinitionBase
    {
       

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new StaticObjectComponent());
        }
    }
}