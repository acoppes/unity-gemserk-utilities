using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public struct PhysicsEnablerComponent : ITagComponent
    {
        
    }
    
    public class PhysicsDisablerOptionalSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Physics2dComponent, PhysicsEnablerComponent>, Exc<DisabledComponent>> enabledFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, PhysicsEnablerComponent, DisabledComponent>> disabledFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in enabledFilter.Value)
            {
                var physics2DComponent = enabledFilter.Pools.Inc1.Get(entity);
                physics2DComponent.gameObject.SetActive(true);
            }
            
            foreach (var entity in disabledFilter.Value)
            {
                var physics2DComponent = disabledFilter.Pools.Inc1.Get(entity);
                physics2DComponent.gameObject.SetActive(false);
            }
        }
    }
}