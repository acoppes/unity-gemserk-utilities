using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class HitVfxSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent, PositionComponent, HitBoxComponent>, Exc<DisabledComponent>> hitFilter1 = default;
        readonly EcsFilterInject<Inc<HealthComponent, AttachPointsComponent>, Exc<DisabledComponent>> hitFilter2 = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in hitFilter1.Value)
            {
                ref var health = ref hitFilter1.Pools.Inc1.Get(entity);

                if (health.processedDamages.Count == 0)
                {
                    continue;
                }
                
                var position = hitFilter1.Pools.Inc2.Get(entity);
                var hitBox = hitFilter1.Pools.Inc3.Get(entity);

                for (int i = 0; i < health.processedDamages.Count; i++)
                {
                    var hit = health.processedDamages[i];

                    if (hit.vfxDefinition == null)
                    {
                        continue;
                    }

                    var hitVfxEntity = world.CreateEntity(hit.vfxDefinition);
                    ref var hitVfxPosition = ref world.GetComponent<PositionComponent>(hitVfxEntity);

                    hitVfxPosition.value = position.value;
                    hitVfxPosition.value.x += UnityEngine.Random.Range(-hitBox.hurt.size.x, hitBox.hurt.size.x) * 0.5f;
                    hitVfxPosition.value.y += UnityEngine.Random.Range(0, hitBox.hurt.size.y);
                    hitVfxPosition.value.z = position.value.z - 0.01f;
                    
                    // if (world.HasComponent<VfxComponent>(hitVfxEntity))
                    // {
                    //     ref var hitVfxComponent = ref world.GetComponent<VfxComponent>(hitVfxEntity);
                    //     hitVfxComponent.delay = UnityEngine.Random.Range(0.0f, maxRandomDelay);
                    // }
                }
            }
            
            foreach (var entity in hitFilter2.Value)
            {
                ref var health = ref hitFilter2.Pools.Inc1.Get(entity);

                if (health.processedDamages.Count == 0)
                {
                    continue;
                }
                
                var attachPoints = hitFilter2.Pools.Inc2.Get(entity);

                if (attachPoints.attachPoints.TryGetValue("hit", out var hitAttachPoint))
                {
                    for (int i = 0; i < health.processedDamages.Count; i++)
                    {
                        var hit = health.processedDamages[i];

                        if (hit.vfxDefinition == null)
                        {
                            continue;
                        }

                        var hitVfxEntity = world.CreateEntity(hit.vfxDefinition);
                        ref var hitVfxPosition = ref world.GetComponent<PositionComponent>(hitVfxEntity);

                        hitVfxPosition.value = hitAttachPoint.position;
                        // hitVfxPosition.value.x += UnityEngine.Random.Range(-hitBox.hurt.size.x, hitBox.hurt.size.x) * 0.5f;
                        // hitVfxPosition.value.y += UnityEngine.Random.Range(0, hitBox.hurt.size.y);
                        hitVfxPosition.value.z -= 0.01f;
                    }
                }

              
            }
        }
    }
}