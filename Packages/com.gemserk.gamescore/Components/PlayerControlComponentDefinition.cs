using Gemserk.Leopotam.Ecs;
using UnityEngine.Serialization;

namespace Game.Components
{
    public struct PlayerControlComponent : IEntityComponent
    {
        public int controlId;
    }
    
    public class PlayerControlComponentDefinition : ComponentDefinitionBase
    {
        [FormerlySerializedAs("player")] 
        public int controlId;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PlayerControlComponent()
            {
                controlId = controlId
            });
        }
    }
}