using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class DecalSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DecalComponent>, Exc<DisabledComponent>> decalFilter = default;
        readonly EcsFilterInject<Inc<DecalComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyableFilter = default;
        readonly EcsFilterInject<Inc<DecalComponent, ModelComponent>, Exc<DisabledComponent>> modelFilter = default;
        
        public void Run(EcsSystems systems)
        {
            var dt = this.dt;
            
            foreach (var entity in decalFilter.Value)
            {
                ref var decalComponent = ref decalFilter.Pools.Inc1.Get(entity);
                
                if (decalComponent.phases.IsEmpty())
                {
                    continue;
                }
                
                decalComponent.ttl.Increase(dt);

                if (decalComponent.ttl.IsReady)
                {
                    decalComponent.phases.Decrease(1);
                 
                    if (decalComponent.phases.IsEmpty())
                    {
                        continue;
                    }

                    decalComponent.ttl.Reset();
                }
            }

            foreach (var entity in destroyableFilter.Value)
            {
                var decalComponent = destroyableFilter.Pools.Inc1.Get(entity);
                ref var destroyableComponent = ref destroyableFilter.Pools.Inc2.Get(entity);

                if (decalComponent.ttl.IsReady)
                {
                    if (decalComponent.phases.IsEmpty())
                    {
                        destroyableComponent.destroy = true;
                    }
                }
            }
            
            foreach (var entity in modelFilter.Value)
            {
                var decalComponent = modelFilter.Pools.Inc1.Get(entity);
                ref var modelComponent = ref modelFilter.Pools.Inc2.Get(entity);

                var color = modelComponent.color;
                color.a = decalComponent.phases.Progress();
                modelComponent.color = color;
            }
        }
    }
}