using System;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    [Serializable]
    public struct JumpComponent : IEntityComponent
    {
        public float speed;
        public float maxTime;
        public float minTime;
        public float jumpBufferTime;
    }

    public class JumpComponentDefinition : ComponentDefinitionBase
    {
        public JumpComponent jumpComponent;

        public override string GetComponentName()
        {
            return nameof(JumpComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, jumpComponent);
        }
    }
}