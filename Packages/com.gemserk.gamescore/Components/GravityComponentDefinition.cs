using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct GravityComponent : IEntityComponent
    {
        public bool disabled;
        public float scale;
        
        public bool inContactWithGround;
        public float groundContactTime;
        public float timeSinceGroundContact;
    }
    
    public class GravityComponentDefinition : ComponentDefinitionBase
    {
        public float scale = 1.0f;

        public override string GetComponentName()
        {
            return nameof(GravityComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new GravityComponent()
            {
                scale = scale
            });
        }
    }
}