using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct RespawnableComponent : IEntityComponent
    {
        public bool enableOnRespawn;
        public bool refillHealthOnRespawn;
        public bool signal;
        
        public int respawnCount;
    }
    
    public struct RespawnActionComponent : IActionComponent
    {
        
    }
    
    public struct RespawnEventComponent : IEventComponent
    {
        
    }
    
    public class RespawnableComponentDefinition : ComponentDefinitionBase
    {
        public bool enableOnRespawn;
        public bool refillHealthOnRespawn;
        public bool signal;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new RespawnableComponent()
            {
                enableOnRespawn = enableOnRespawn,
                refillHealthOnRespawn = refillHealthOnRespawn,
                signal = signal
            });
        }
    }
}