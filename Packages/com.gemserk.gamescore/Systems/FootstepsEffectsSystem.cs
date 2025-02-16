using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class FootstepsEffectsSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<FootstepsComponent>, Exc<DisabledComponent>> footstepFilter = default;
        readonly EcsFilterInject<Inc<FootstepsComponent, MovementComponent>, Exc<DisabledComponent>> movementFilter = default;
        readonly EcsFilterInject<Inc<FootstepsComponent, Physics2dComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<FootstepsComponent, GravityComponent>, Exc<DisabledComponent>> gravityFilter = default;
        readonly EcsFilterInject<Inc<FootstepsComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<FootstepsComponent>(entity))
            {
                ref var footsteps = ref world.GetComponent<FootstepsComponent>(entity);
                if (footsteps.particlesPrefab != null)
                {
                    var walkingParticlesInstance = GameObject.Instantiate(footsteps.particlesPrefab);
                    footsteps.walkingParticleSystem = walkingParticlesInstance.GetComponent<ParticleSystem>();
                    walkingParticlesInstance.SetActive(true);
                    footsteps.walkingParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<FootstepsComponent>(entity))
            {
                ref var footsteps = ref world.GetComponent<FootstepsComponent>(entity);
                if (footsteps.walkingParticleSystem != null)
                {
                    GameObject.Destroy(footsteps.walkingParticleSystem.gameObject);
                    footsteps.walkingParticleSystem = null;
                }
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var e in footstepFilter.Value)
            {
                ref var footsteps = ref footstepFilter.Pools.Inc1.Get(e);
                footsteps.isWalking = false;
            }
            
            foreach (var e in movementFilter.Value)
            {
                ref var footsteps = ref movementFilter.Pools.Inc1.Get(e);
                var movement = movementFilter.Pools.Inc2.Get(e);
                footsteps.isWalking = footsteps.isWalking || movement.currentVelocity.sqrMagnitude > GameConstants.MinSpeedMovement;
            }
            
            foreach (var e in physicsFilter.Value)
            {
                ref var footsteps = ref physicsFilter.Pools.Inc1.Get(e);
                var physics2d = physicsFilter.Pools.Inc2.Get(e);
                footsteps.isWalking = footsteps.isWalking || physics2d.velocity.sqrMagnitude >
                                      GameConstants.MinSpeedMovement;
            }
            
            foreach (var e in gravityFilter.Value)
            {
                ref var footsteps = ref gravityFilter.Pools.Inc1.Get(e);
                var gravity = gravityFilter.Pools.Inc2.Get(e);
                footsteps.isWalking = footsteps.isWalking && gravity.inContactWithGround;
            }
            
            foreach (var e in filter.Value)
            {
                ref var footsteps = ref filter.Pools.Inc1.Get(e);
                var position = filter.Pools.Inc2.Get(e);

                if (footsteps.disabled)
                { 
                    if (footsteps.walkingParticleSystem != null)
                    {
                        if (footsteps.walkingParticleSystem.isPlaying )
                        {
                            footsteps.walkingParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                        }
                    }
                    
                    continue;
                }
                
                if (footsteps.walkingParticleSystem != null)
                {
                    if (position.type == 0)
                    {
                        footsteps.walkingParticleSystem.transform.position =
                            GamePerspective.ConvertFromWorld(position.value);
                    } else if (position.type == 1)
                    {
                        footsteps.walkingParticleSystem.transform.position = position.value;
                    }
                    
                    if (!footsteps.walkingParticleSystem.isPlaying && footsteps.isWalking)
                    {
                        footsteps.walkingParticleSystem.Play(true);
                    }
                    else if (footsteps.walkingParticleSystem.isPlaying && !footsteps.isWalking)
                    {
                        footsteps.walkingParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                    

                }

                var positionXZ = position.value.XZ();
                
                if (footsteps.lastSpawnPosition.DistanceSqr(positionXZ) >
                    footsteps.spawnDistance.Square())
                {
                    footsteps.lastSpawnPosition = positionXZ;

                    var randomPosition = position.value +
                                         RandomExtensions.RandomVectorXZ(0, footsteps.randomOffset,
                                             0, 360);
                    
                    if (footsteps.decalDefinition != null)
                    {
                        var decal = world.CreateEntity(footsteps.decalDefinition);
                        ref var decalPosition = ref world.GetComponent<PositionComponent>(decal);
                        decalPosition.value = randomPosition;
                    }
                    
                    if (footsteps.sfxDefinition != null)
                    {
                        var sfxEntity = world.CreateEntity(footsteps.sfxDefinition);
                        ref var sfxPosition = ref world.GetComponent<PositionComponent>(sfxEntity);
                        sfxPosition.value = randomPosition;
                    }
                }
                
            }
        }

    }
}