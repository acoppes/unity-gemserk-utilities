using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct VfxComponent : IEntityComponent
    {
        // public enum FollowType
        // {
        //     InitialPosition,
        //     CurrentPosition
        // }
        
        public string animation;

        public Entity target;
        // public FollowType followType;
    }
    
    public class VfxComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new VfxComponent
            {

            });
        }
    }
}