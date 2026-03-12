using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class RespawnSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RespawnableComponent, RespawnActionComponent>> respawnablesFilter = default;
        private readonly EcsFilterInject<Inc<RespawnableComponent, DisabledComponent, RespawnActionComponent>> respawnablesDisabled = default;
        private readonly EcsFilterInject<Inc<RespawnableComponent, HealthComponent, RespawnActionComponent>> respawnablesWithHealth = default;
        private readonly EcsFilterInject<Inc<RespawnActionComponent>> respawnActions = default;
        private readonly EcsFilterInject<Inc<RespawnEventComponent>> respawnEvents = default;

        public SignalAsset respawnSignal;
        
        public void Run(EcsSystems systems)
        {
            // at the start, consume all the events from previous frame
            foreach (var e in respawnEvents.Value)
            {
                respawnEvents.Pools.Inc1.Del(e);
            }
            
            foreach (var e in respawnablesFilter.Value)
            {
                ref var respawnable = ref respawnablesFilter.Pools.Inc1.Get(e);
                respawnable.respawnCount++;

                world.AddComponent(e, new RespawnEventComponent());
            }
            
            foreach (var e in respawnablesWithHealth.Value)
            {
                var respawnable = respawnablesWithHealth.Pools.Inc1.Get(e);
                ref var health = ref respawnablesWithHealth.Pools.Inc2.Get(e);
                
                if (respawnable.refillHealthOnRespawn)
                {
                    health.current = health.total;
                }
            }
            
            foreach (var e in respawnablesDisabled.Value)
            {
                var respawnable = respawnablesDisabled.Pools.Inc1.Get(e);
                if (respawnable.enableOnRespawn)
                {
                    respawnablesDisabled.Pools.Inc2.Del(e);
                }
            }
            
            if (respawnSignal)
            {
                foreach (var e in respawnablesFilter.Value)
                {
                    var respawnable = respawnablesFilter.Pools.Inc1.Get(e);
                    if (respawnable.signal)
                    {
                        respawnSignal.Signal(world.GetEntity(e));
                    }
                }
            }
            
            // at the end consume all the processed actions
            foreach (var e in respawnActions.Value)
            {
                respawnActions.Pools.Inc1.Del(e);
            }
        }
    }
}