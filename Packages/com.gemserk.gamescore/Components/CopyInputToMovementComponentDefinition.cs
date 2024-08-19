using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct CopyInputToMovementComponent : IEntityComponent
    {
        public float fixedAngles;
    }
    
    public class CopyInputToMovementComponentDefinition : ComponentDefinitionBase
    {
        public float fixedAngles = 0;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CopyInputToMovementComponent()
            {
                fixedAngles = fixedAngles
            });
        }
    }
}