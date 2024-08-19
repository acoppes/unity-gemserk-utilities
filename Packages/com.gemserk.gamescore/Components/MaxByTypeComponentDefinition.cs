using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public interface ITypeMax
    {
        public int GetMax();
    }
    
    public struct MaxByTypeComponent : IEntityComponent
    {
        public int updates;
        public ITypeMax type;
    }
    
    public class MaxByTypeComponentDefinition : ComponentDefinitionBase
    {
        public TypeMaxAsset typeMax;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new MaxByTypeComponent()
            {
                type = typeMax
            });
        }
    }
}