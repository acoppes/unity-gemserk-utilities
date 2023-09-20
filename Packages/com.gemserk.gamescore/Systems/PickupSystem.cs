using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PickupSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PickupComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<PickupComponent, PositionComponent>, Exc<DisabledComponent>> pickupFilter = default;
        readonly EcsFilterInject<Inc<CanPickupComponent, PositionComponent>, Exc<DisabledComponent>> canPickupFilter = default;
        
        // public SignalAsset onPickupSignal;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var pickup = ref pickupFilter.Pools.Inc1.Get(e);
                pickup.wasPicked = pickup.picked;
            }
            
            foreach (var e1 in pickupFilter.Value)
            {
                ref var pickup = ref pickupFilter.Pools.Inc1.Get(e1);
                var pickupPosition = pickupFilter.Pools.Inc2.Get(e1);

                if (pickup.picked)
                {
                    continue;
                }
                    
                pickup.pickedByEntities.Clear();
                
                foreach (var e2 in canPickupFilter.Value)
                {
                    ref var canPickup = ref canPickupFilter.Pools.Inc1.Get(e2);
                    var canPickupPosition = canPickupFilter.Pools.Inc2.Get(e2);

                    if (Vector3.SqrMagnitude(canPickupPosition.value - pickupPosition.value) < canPickup.sqrRange)
                    {
                        // pickup.picked = true;
                        pickup.pickedByEntities.Add(world.GetEntity(e2));
                        
                        // canPickup.pickups.Add(pickup.ToData());
                        // if (onPickupSignal != null)
                        // {
                        //     onPickupSignal.Signal((world.GetEntity(e1)));
                        // }
                    }
                }
            }
        }
    }
}