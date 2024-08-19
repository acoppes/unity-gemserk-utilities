using Game.Systems;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class PositionComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public enum StartPositionType
        {
            None = 0,
            CopyFromTransform = 1,
            CopyFromTransformTo3dWorld = 2,
        }

        public StartPositionType startingPositionType = StartPositionType.None;
        
        // this is used internally to be treated as half 3d or 2d.
        public int type;

        public override void Apply(World world, Entity entity)
        {
            if (!entity.Has<PositionComponent>())
            {
                world.AddComponent(entity, new PositionComponent()
                {
                    type = type
                });
            }

            ref var position = ref entity.Get<PositionComponent>();
            position.type = type;
            
            if (startingPositionType == StartPositionType.CopyFromTransformTo3dWorld)
            {
                position.value = GamePerspective.ConvertToWorld(transform.position);
            } else if (startingPositionType == StartPositionType.CopyFromTransform)
            {
                position.value = transform.position;
            }
        }
    }
}