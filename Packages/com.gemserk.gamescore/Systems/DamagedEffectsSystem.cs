using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class DamagedEffectsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DamageEffectsComponent, PositionComponent>, Exc<AttachPointsComponent, DisabledComponent>> preFilter = default;
        readonly EcsFilterInject<Inc<DamageEffectsComponent, HealthComponent, PositionComponent, AttachPointsComponent>, Exc<DisabledComponent>> attachPointsFilter = default;
        readonly EcsFilterInject<Inc<DamageEffectsComponent, HealthComponent>, Exc<DisabledComponent>> filter = default;

        public void Run(EcsSystems systems)
        {
            // by default set damage vfx positions to be in the position of the entity
            foreach (var e in preFilter.Value)
            {
                ref var damageEffects = ref preFilter.Pools.Inc1.Get(e);
                var position = preFilter.Pools.Inc2.Get(e);
                
                for (var i = 0; i < damageEffects.onDamageEffects.Length; i++)
                {
                    damageEffects.onDamageEffects[i].position = position.value;
                }
                
                for (var i = 0; i < damageEffects.onDeathEffects.Length; i++)
                {
                    damageEffects.onDeathEffects[i].position = position.value;
                }
            }

            foreach (var e in attachPointsFilter.Value)
            {
                ref var damageEffects = ref attachPointsFilter.Pools.Inc1.Get(e);
                var health = attachPointsFilter.Pools.Inc2.Get(e);
                var position = attachPointsFilter.Pools.Inc3.Get(e);
                var attachPoints = attachPointsFilter.Pools.Inc4.Get(e);

                if (health.processedDamages.Count > 0)
                {
                    // TODO: only update on spawn, or maybe delegate to vfx instance

                    for (var i = 0; i < damageEffects.onDamageEffects.Length; i++)
                    {
                        var vfxData = damageEffects.onDamageEffects[i];
                    
                        if (vfxData.positionType == VfxComponentData.PositionType.Ground)
                        {
                            vfxData.position = position.value.SetY(0);
                        } else if (vfxData.positionType == VfxComponentData.PositionType.Center)
                        {
                            vfxData.position = position.value;
                        }
                        else if (vfxData.positionType == VfxComponentData.PositionType.AttachPoint)
                        {
                            if (attachPoints.attachPoints.TryGetValue(vfxData.attachPoint, out var attachPoint))
                            {
                                vfxData.position = attachPoint.position;
                            }
                        }

                        damageEffects.onDamageEffects[i] = vfxData;
                    }
                }
                
                if (health.wasKilledLastFrame)
                {
                    for (var i = 0; i < damageEffects.onDeathEffects.Length; i++)
                    {
                        var vfxData = damageEffects.onDeathEffects[i];
                    
                        if (vfxData.positionType == VfxComponentData.PositionType.Ground)
                        {
                            vfxData.position = position.value.SetY(0);
                        } else if (vfxData.positionType == VfxComponentData.PositionType.Center)
                        {
                            vfxData.position = position.value;
                        } else if (vfxData.positionType == VfxComponentData.PositionType.AttachPoint)
                        {
                            if (attachPoints.attachPoints.TryGetValue(vfxData.attachPoint, out var attachPoint))
                            {
                                vfxData.position = attachPoint.position;
                            }
                        }

                        damageEffects.onDeathEffects[i] = vfxData;
                    }
                }

                // locate the damage effect in the proper position
            }
            
            foreach (var e in filter.Value)
            {
                // if damaged, spawn effects, should it have cooldown?
                
                var damageEffects = filter.Pools.Inc1.Get(e);
                var health = filter.Pools.Inc2.Get(e);

                var entity = world.GetEntity(e);
                
                if (health.processedDamages.Count > 0)
                {
                    // it spawns a generic damage effect given a damage
                    // in the future could check for damage type
                
                    for (var i = 0; i < damageEffects.onDamageEffects.Length; i++)
                    {
                        var vfxData = damageEffects.onDamageEffects[i];

                        var offset = Vector3.zero;

                        if (vfxData.randomOffsetType == VfxComponentData.RandomOffsetType.PlaneXZ)
                        {
                            var random = UnityEngine.Random.insideUnitCircle * vfxData.range;
                            offset = new Vector3(random.x, 0, random.y);
                        }
                    
                        var vfxEntity = world.CreateEntity(vfxData.definition);
                        vfxEntity.Get<PositionComponent>().value = vfxData.position + offset;

                        if (entity.Has<PlayerComponent>() && vfxEntity.Has<PlayerComponent>())
                        {
                            vfxEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;
                        }
                    }
                }
                
                // in the case of changing alive state, dont require to jave processed damages to consider effect
                if (health.wasKilledLastFrame)
                {
                    for (var i = 0; i < damageEffects.onDeathEffects.Length; i++)
                    {
                        var vfxData = damageEffects.onDeathEffects[i];
                        
                        var offset = Vector3.zero;

                        if (vfxData.randomOffsetType == VfxComponentData.RandomOffsetType.PlaneXZ)
                        {
                            var random = UnityEngine.Random.insideUnitCircle * vfxData.range;
                            offset = new Vector3(random.x, 0, random.y);
                        }
                    
                        var vfxEntity = world.CreateEntity(vfxData.definition);
                        vfxEntity.Get<PositionComponent>().value = vfxData.position + offset;
                        
                        if (entity.Has<PlayerComponent>() && vfxEntity.Has<PlayerComponent>())
                        {
                            vfxEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;
                        }
                    }
                }
            }
            
           
        }
    }
}