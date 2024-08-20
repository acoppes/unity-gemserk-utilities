using Game.Components;
using Game.Models;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Definitions
{
    public class UnitInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public enum PositionType
        {
            None = 0,
            CopyFromTransform3d = 1,
            CopyFromTransform2d = 2,
        }
        
        public enum NamingType
        {
            None = 0,
            String = 1,
            CopyFromGameObject = 2
        }

        [Separator("Identify")] 
        public NamingType namingType = NamingType.None;
        [ConditionalField(nameof(namingType), false, NamingType.String)]
        public string entityName;
        [ConditionalField(nameof(namingType), true, NamingType.None)]
        public bool singleton;
        [ConditionalField(nameof(namingType), false, NamingType.String)]
        public bool disableSpawnNameOverride;

        [Separator("Location")] 
        public PositionType positionType = PositionType.None;

        [Separator("Player Input")] 
        public bool controllable = false;
        [ConditionalField(nameof(controllable))]
        public int playerInput;

        [Separator("Player Team")] 
        public bool overridePlayer;
        [ConditionalField(nameof(overridePlayer))]
        public int team;
        
        [Separator("Looking Direction")] 
        public bool overrideLookingDirection = false;

        [ConditionalField(nameof(overrideLookingDirection))]
        public LookingDirectionComponentDefinition.StartingLookingDirectionType startingLookingDirectionType =
            LookingDirectionComponentDefinition.StartingLookingDirectionType.Fixed;
        
        [ConditionalField(nameof(overrideLookingDirection))]
        public Vector3 startLookingDirection;
        
        [Separator("Model")]
        public bool overrideModel;
        [ConditionalField(nameof(overrideModel))]
        public GameObject modelInstance;
        [ConditionalField(nameof(overrideModel))]
        public int startingTextureVariant;
        [ConditionalField(nameof(overrideModel))]
        public bool overridePositionFromModel;

        [Separator("Animation")] 
        public StartingAnimationComponent.StartingAnimationType startingAnimationType = StartingAnimationComponent.StartingAnimationType.None;
        [ConditionalField(nameof(startingAnimationType), false, StartingAnimationComponent.StartingAnimationType.Name)]
        public string startingAnimation;
        [ConditionalField(nameof(startingAnimationType), true, StartingAnimationComponent.StartingAnimationType.None)]
        public bool randomizeStartFrame;
        [ConditionalField(nameof(startingAnimationType), true, StartingAnimationComponent.StartingAnimationType.None)]
        public bool startLooping = true;
        
        [FormerlySerializedAs("overrideHitPoints")] [Separator("HitPoints")]
        public bool overrideHealth;
        [FormerlySerializedAs("hitPoints")] [ConditionalField(nameof(overrideHealth))]
        public float health;
        
        public void Apply(World world, Entity entity)
        {
            if (positionType != PositionType.None)
            {
                if (!world.HasComponent<PositionComponent>(entity))
                {
                    world.AddComponent(entity, new PositionComponent());
                } 
            }
            
            if (positionType == PositionType.CopyFromTransform3d)
            {
                ref var position = ref world.GetComponent<PositionComponent>(entity);
                position.value = GamePerspective.ConvertToWorld(transform.position);
                position.type = 0;
            } else if (positionType == PositionType.CopyFromTransform2d)
            {
                ref var position = ref world.GetComponent<PositionComponent>(entity);
                position.value = transform.position;
                position.type = 1;
            }
            
            if (controllable)
            {
                if (!entity.Has<PlayerInputComponent>())
                {
                    world.AddComponent(entity, new PlayerInputComponent());    
                }

                ref var playerInputComponent = ref entity.Get<PlayerInputComponent>();
                playerInputComponent.playerInput = playerInput;
                
                if (entity.Has<ControllableByComponent>())
                {
                    ref var controllableComponent = ref entity.Get<ControllableByComponent>();
                    controllableComponent.playerControlId = playerInput;
                }
                
            }

            if (overrideLookingDirection && world.HasComponent<LookingDirection>(entity))
            {
                ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
                
                if (startingLookingDirectionType == LookingDirectionComponentDefinition.StartingLookingDirectionType.Fixed)
                {
                    lookingDirection.value = startLookingDirection.normalized;
                } else if (startingLookingDirectionType == LookingDirectionComponentDefinition.StartingLookingDirectionType.Random2d)
                {
                    // we could check which type of position we are using and randomize in that world.
                    lookingDirection.value = RandomExtensions.RandomVector2(1, 1, 0, 360);
                }
            }
            
            if (overrideModel && world.HasComponent<ModelComponent>(entity))
            {
                if (world.HasComponent<ModelRemapTexturePerPlayerComponent>(entity))
                {
                    ref var remapComponent = ref world.GetComponent<ModelRemapTexturePerPlayerComponent>(entity);
                    remapComponent.textureVariant = startingTextureVariant;
                }
                
                ref var modelComponent = ref world.GetComponent<ModelComponent>(entity);

                if (modelInstance != null && modelComponent.prefab == null)
                {
                    modelComponent.instance = modelInstance.GetComponent<Model>();

                    if (overridePositionFromModel)
                    {
                        ref var position = ref world.GetComponent<PositionComponent>(entity);
                        position.value =
                            GamePerspective.ConvertToWorld(modelInstance.transform.position);
                    }
                }
            }
            
            if (startingAnimationType != StartingAnimationComponent.StartingAnimationType.None && world.HasComponent<AnimationsComponent>(entity))
            {
                if (!entity.Has<StartingAnimationComponent>())
                {
                    world.AddComponent(entity, new StartingAnimationComponent());
                }

                ref var startingAnimationComponent = ref entity.Get<StartingAnimationComponent>();

                startingAnimationComponent.startingAnimationType = startingAnimationType;
                startingAnimationComponent.randomizeStartFrame = randomizeStartFrame;
                startingAnimationComponent.name = startingAnimation;
                startingAnimationComponent.loop = startLooping;
            }

            if (overrideHealth && world.HasComponent<HealthComponent>(entity))
            {
                ref var health = ref world.GetComponent<HealthComponent>(entity);
                health.total = this.health;
                health.current = this.health;
            }

            if (overridePlayer)
            {
                if (!world.HasComponent<PlayerComponent>(entity))
                {
                    world.AddComponent(entity, new PlayerComponent());
                } 
                
                ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
                playerComponent.player = team;
            }
            
            if (namingType == NamingType.String)
            {
                if (!entity.Has<NameComponent>())
                {
                    entity.Add(new NameComponent());
                }

                ref var nameComponent = ref entity.Get<NameComponent>();
                nameComponent.name = entityName;
                nameComponent.singleton = singleton;
                
            } else if (namingType == NamingType.CopyFromGameObject)
            {
                if (!entity.Has<NameComponent>())
                {
                    entity.Add(new NameComponent());
                }

                ref var nameComponent = ref entity.Get<NameComponent>();
                nameComponent.name = gameObject.name;
                nameComponent.singleton = singleton;
            }
        }

        private void OnValidate()
        {
            if (!gameObject.IsSafeToModifyName() || disableSpawnNameOverride)
                return;

            if (namingType == NamingType.String)
            {
                if (!string.IsNullOrEmpty(entityName))
                {
                    gameObject.name = $"Spawn({entityName})";
                }
                else
                {
                    gameObject.name = "Spawn()"; 
                }
            }
        }
    }
}