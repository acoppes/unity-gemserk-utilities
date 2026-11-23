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
                var physics2DComponent = enabledFilter.Pools.Inc1.Get(entity);

                if (physics2DComponent.gameObject) 
                {
                    if (!physics2DComponent.gameObject.activeSelf)
                    {
                        physics2DComponent.gameObject.SetActive(true);
                    }

                    if (physics2DComponent.collideWithDynamicCollider)
                    {
                        physics2DComponent.collideWithDynamicCollider.enabled = 
                            !physics2DComponent.disableCollisions;
                    }
                    
                    if (physics2DComponent.collideWithStaticCollider)
                    {
                        physics2DComponent.collideWithStaticCollider.enabled = 
                            !physics2DComponent.disableCollisions;
                    }

                    for (var i = 0; i < physics2DComponent.colliders.Count; i++)
                    {
                        var collider = physics2DComponent.colliders[i];
                        collider.enabled = !physics2DComponent.disableCollisions;
                    }
                }
            }
            
            foreach (var entity in disabledFilter.Value)
            {
                var physics2DComponent = disabledFilter.Pools.Inc1.Get(entity);

                if (physics2DComponent.gameObject && physics2DComponent.gameObject.activeSelf) 
                {
                    physics2DComponent.gameObject.SetActive(false);
                }
            }
        }
    }
}