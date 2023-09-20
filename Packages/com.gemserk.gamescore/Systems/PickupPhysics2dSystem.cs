using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PickupPhysics2dSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PickupComponent, Physics2dComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public SignalAsset onPickupSignal;
        
        private Collider2D[] colliders = new Collider2D[1];
        
        public void Run(EcsSystems systems)
        {
            var contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = LayerMask.GetMask("CollideWithStaticObstacles"),
                useTriggers = false
            };
            
            foreach (var entity in physics2dFilter.Value)
            {
                ref var pickup = ref physics2dFilter.Pools.Inc1.Get(entity);
                ref var physics2d = ref physics2dFilter.Pools.Inc2.Get(entity);
                
                pickup.wasPicked = pickup.picked;

                if (pickup.picked)
                {
                    continue;
                }
                
                if (Physics2D.OverlapCollider(physics2d.collideWithDynamicCollider,
                        contactFilter, colliders) > 0)
                {
                    var collider = colliders[0];
                    var entityReference = collider.GetComponentInParent<EntityReference>();
                    if (entityReference != null)
                    {
                        if (world.Exists(entityReference.entity))
                        {
                            // spawn particle

                            if (onPickupSignal != null)
                            {
                                onPickupSignal.Signal((world.GetEntity(entity)));
                            }

                            pickup.picked = true;
                        }
                    }
                }
            }
        }
    }
}