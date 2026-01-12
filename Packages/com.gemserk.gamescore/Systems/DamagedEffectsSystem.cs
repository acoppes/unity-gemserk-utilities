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
                
                for (var i = 0; i < damageEffects.onDamageSpawns.Length; i++)
                {
                    damageEffects.onDamageSpawns[i].position = position.value;
                }
                
                for (var i = 0; i < damageEffects.onDeathSpawns.Length; i++)
                {
                    damageEffects.onDeathSpawns[i].position = position.value;
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

                    for (var i = 0; i < damageEffects.onDamageSpawns.Length; i++)
                    {
                        var spawnData = damageEffects.onDamageSpawns[i];
                    
                        if (spawnData.positionType == SpawnData.PositionType.Ground)
                        {
                            spawnData.position = position.value.SetY(0);
                        } else if (spawnData.positionType == SpawnData.PositionType.Center)
                        {
                            spawnData.position = position.value;
                        }
                        else if (spawnData.positionType == SpawnData.PositionType.AttachPoint)
                        {
                            if (attachPoints.attachPoints.TryGetValue(spawnData.attachPoint, out var attachPoint))
                            {
                                spawnData.position = attachPoint.position;
                            }
                        }

                        damageEffects.onDamageSpawns[i] = spawnData;
                    }
                }
                
                if (health.wasKilledLastFrame)
                {
                    for (var i = 0; i < damageEffects.onDeathSpawns.Length; i++)
                    {
                        var spawnData = damageEffects.onDeathSpawns[i];
                    
                        if (spawnData.positionType == SpawnData.PositionType.Ground)
                        {
                            spawnData.position = position.value.SetY(0);
                        } else if (spawnData.positionType == SpawnData.PositionType.Center)
                        {
                            spawnData.position = position.value;
                        } else if (spawnData.positionType == SpawnData.PositionType.AttachPoint)
                        {
                            if (attachPoints.attachPoints.TryGetValue(spawnData.attachPoint, out var attachPoint))
                            {
                                spawnData.position = attachPoint.position;
                            }
                        }

                        damageEffects.onDeathSpawns[i] = spawnData;
                    }
                }

                // locate the damage effect in the proper position
            }
            
            foreach (var e in filter.Value)
            {
                // if damaged, spawn effects, should it have cooldown?
                
                var damageEffects = filter.Pools.Inc1.Get(e);
                var health = filter.Pools.Inc2.Get(e);
                
                if (health.processedDamages.Count > 0)
                {
                    // it spawns a generic damage effect given a damage
                    // in the future could check for damage type
                
                    var entity = world.GetEntity(e);
                    
                    for (var i = 0; i < damageEffects.onDamageSpawns.Length; i++)
                    {
                        var spawnData = damageEffects.onDamageSpawns[i];

                        if (spawnData.disabled)
                        {
                            continue;
                        }

                        var offset = Vector3.zero;

                        if (spawnData.randomOffsetType == SpawnData.RandomOffsetType.PlaneXZ)
                        {
                            var random = UnityEngine.Random.insideUnitCircle * spawnData.range;
                            offset = new Vector3(random.x, 0, random.y);
                        }
                    
                        var spawnedEntity = world.CreateEntity(spawnData.definition, new IEntityInstanceParameter[]
                        {
                            new SourceEntityParameter(entity)
                        });
                        
                        spawnedEntity.Get<PositionComponent>().value = spawnData.position + offset;

                        // if (entity.Has<PlayerComponent>() && spawnedEntity.Has<PlayerComponent>())
                        // {
                        //     spawnedEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;
                        // }
                    }
                }
                
                // in the case of changing alive state, dont require to jave processed damages to consider effect
                if (health.wasKilledLastFrame)
                {
                    var entity = world.GetEntity(e);
                    
                    for (var i = 0; i < damageEffects.onDeathSpawns.Length; i++)
                    {
                        var spawnData = damageEffects.onDeathSpawns[i];
                        
                        if (spawnData.disabled)
                        {
                            continue;
                        }
                        
                        var offset = Vector3.zero;

                        if (spawnData.randomOffsetType == SpawnData.RandomOffsetType.PlaneXZ)
                        {
                            var random = UnityEngine.Random.insideUnitCircle * spawnData.range;
                            offset = new Vector3(random.x, 0, random.y);
                        }
                    
                        var spawnedEntity = world.CreateEntity(spawnData.definition, new IEntityInstanceParameter[]
                        {
                            new SourceEntityParameter(entity)
                        });
                        
                        spawnedEntity.Get<PositionComponent>().value = spawnData.position + offset;
                        
                        // if (entity.Has<PlayerComponent>() && spawnedEntity.Has<PlayerComponent>())
                        // {
                        //     spawnedEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;
                        // }
                        
                        // copy modifier to spawned entity? maybe this should be in the opposite way, like, on spawn I set the
                        // spawnedFrom entity component or sourceentity component, and there is a system or something there
                        // processing this kind of that so I can grow over that without having to modify this code and/or?
                        // without having to copy paste this code everywhere....
                        
                        // if (entity.Has<StatsModifiersComponent>() && spawnedEntity.Has<StatsModifiersComponent>())
                        // {
                        //     var modifiers = entity.Get<StatsModifiersComponent>();
                        //
                        //     foreach (var modifier in modifiers.statsModifiers)
                        //     {
                        //         if (modifier.state != StatsModifier.State.Inactive)
                        //         {
                        //             spawnedEntity.Get<StatsModifiersComponent>().Add(modifier);
                        //         }
                        //     }
                        // }
                    }
                }
            }
            
           
        }
    }
}