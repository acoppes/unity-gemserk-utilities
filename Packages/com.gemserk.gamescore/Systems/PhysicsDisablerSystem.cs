using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class PhysicsDisablerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Physics2dComponent>, Exc<DisabledComponent>> enabledFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, DisabledComponent>> disabledFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in enabledFilter.Value)
            {
                var physics2dComponent = enabledFilter.Pools.Inc1.Get(entity);

                if (physics2dComponent.gameObject != null) 
                {
                    if (!physics2dComponent.gameObject.activeSelf)
                    {
                        physics2dComponent.gameObject.SetActive(true);
                    }

                    if (physics2dComponent.collideWithDynamicCollider != null)
                    {
                        physics2dComponent.collideWithDynamicCollider.enabled = 
                            !physics2dComponent.disableCollisions;
                    }
                    
                    if (physics2dComponent.collideWithStaticCollider != null)
                    {
                        physics2dComponent.collideWithStaticCollider.enabled = 
                            !physics2dComponent.disableCollisions;
                    }

                    foreach (var collider in physics2dComponent.colliders)
                    {
                        collider.enabled = !physics2dComponent.disableCollisions;
                    }
                }
            }
            
            foreach (var entity in disabledFilter.Value)
            {
                var physics2dComponent = disabledFilter.Pools.Inc1.Get(entity);

                if (physics2dComponent.gameObject != null && physics2dComponent.gameObject.activeSelf) 
                {
                    physics2dComponent.gameObject.SetActive(false);
                }
            }
        }
    }
}