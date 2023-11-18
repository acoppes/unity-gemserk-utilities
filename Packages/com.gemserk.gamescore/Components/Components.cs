using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Controllers;
using Game.Definitions;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public struct PlayerInputComponent : IEntityComponent
    {
        public bool disabledByControllable;
        public int playerInput;
        public bool isControlled;

        public Dictionary<string, InputAction> inputActions;
    }
    
    public struct BufferedInputComponent : IEntityComponent
    {
        public const float DefaultBufferTime = 0.2f;
        public const int MaxBufferCount = 15;

        public List<string> buffer;
        
        public float currentBufferTime;
        public float totalBufferTime;

        public static BufferedInputComponent Default()
        {
            return new BufferedInputComponent()
            {
                buffer = new List<string>(),
                totalBufferTime = DefaultBufferTime
            };
        }

        public bool HasBufferedAction(InputComponent.InputAction inputAction)
        {
            return HasBufferedActions(inputAction.name);
        }
        
        public bool HasBufferedAction(InputComponent.InputAction inputAction1, InputComponent.InputAction inputAction2)
        {
            return HasBufferedActions(inputAction1.name, inputAction2.name);
        }

        public bool HasBufferedActions(params string[] actions)
        {
            if (actions.Length == 0)
            {
                return false;
            }

            var bufferStart = buffer.Count - actions.Length;
            
            if (bufferStart < 0)
            {
                return false;
            }

            for (var i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                if (!buffer[bufferStart + i].Equals(action, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        public void InsertInBuffer(string action)
        {
            buffer.Add(action);
            currentBufferTime = totalBufferTime;
        }

        public void ConsumeBuffer()
        {
            buffer.Clear();
        }
        
        // public void ClearInsertInBuffer()
        // {
        //     foreach (var buttonName in actions.Keys)
        //     {
        //         actions[buttonName].shouldInsertInBuffer = false;
        //     }
        // }

    }

    public struct LookingDirectionParameter : IEntityInstanceParameter
    {
        public Vector3 value;

        public void Apply(World world, Entity entity)
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.value = value;
        }
    }

    public struct ModelEnabledComponent : IEntityComponent
    {
        
    }

    public struct ModelComponent : IEntityComponent
    {
        public enum Visiblity
        {
            Visible = 0,
            Hidden = 1
        }
        
        public enum RotationType
        {
            FlipToLookingDirection = 0,
            Rotate = 1,
            None = 2
        }
        
        public enum SortingLayerType
        {
            None = 0,
            CopyFromComponent = 1
        }
        
        public GameObject prefab;
        
        public Model instance;
        public GameObject modelGameObject;

        // TODO: cache this on creation
        public bool hasSubModelObject;

        public RotationType rotation;
        public bool flipUseRotation;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;

        public Color color;

        public SortingLayerType sortingLayerType;
        public int sortingOrder;
        public int sortingLayer;
    }

    public struct ModelShakeComponent : IEntityComponent
    {
        public float duration;
        public float time;

        public float updateTime;

        public bool restart;
        
        public Vector3 currentOffset;

        public float intensity;

        public void Shake(float t, float intensity = 1)
        {
            duration = t;
            time = 0;
            restart = true;
            this.intensity = intensity;
        }
    }
    
    public struct CurrentAnimationAttackComponent : IEntityComponent
    {
        public int animation;
        public int frame;
        
        public bool currentFrameHit;
        // public float cancellationTime;
    }

    public struct DamageData
    {
        public Vector3 position;
        public float value;
        public Entity source;

        public bool knockback;
        public IEntityDefinition vfxDefinition;
    }

    public struct HealthComponent : IEntityComponent
    {
        public delegate void OnEntityEventHandler(World world, Entity entity);
        
        [Flags]
        public enum AliveType
        {
            None = 0,
            Alive = 1 << 1,
            Death = 1 << 2
        }
        
        public float total;
        public float current;

        public float timeSinceLastHit;

        public Cooldown temporaryInvulnerability;
        
        public int invulnerableCount;
        public bool invulnerable => invulnerableCount > 0;

        public AliveType previousAliveState;
        public AliveType aliveType => current > 0 ? AliveType.Alive : AliveType.Death;

        public bool wasKilledLastFrame => previousAliveState == AliveType.Alive &&
                                         aliveType == AliveType.Death;

        public bool autoDestroyOnDeath;
        public bool autoDisableOnDeath;

        public bool triggerForceDeath;
        
        public bool IsFull() => Mathf.Approximately(current, total);

        public float factor
        {
            get => current / total;
            set => current = total * value;
        }

        // public bool interrumpible;
        
        public List<DamageData> damages;
        public List<DamageData> processedDamages;
        
        public List<DamageData> healEffects;
        
        public event OnEntityEventHandler onDamageEvent;
        public event OnEntityEventHandler onDeathEvent;

        public void OnDamageEvent(World world, Entity entity)
        {
            if (onDamageEvent != null)
            {
                onDamageEvent(world, entity);
            }
        }

        public void OnDeathEvent(World world, Entity entity)
        {
            if (onDeathEvent != null)
            {
                onDeathEvent(world, entity);
            }
        }

        public void ClearEvents()
        {
            onDamageEvent = null;
            onDeathEvent = null;
        }
    }
    
    public struct HealthRegenerationComponent : IEntityComponent
    {
        public bool enabled;
        public Cooldown tick;
        public float regenerationPerTick;
    }
    
    public struct HealthBarComponent : IEntityComponent
    {
        public HealthBarModel instance;
        public Vector3 offset;
        public int size;
        // public bool disabled;

        public int player;
        public Vector3 position;
        public float factor;
        public bool visible;
        public bool highlighted;
    }



    public struct ActiveControllerComponent : IEntityComponent
    {
        public float controlledTime;
        public float lastControlledTime;

        public int controlledFrames;
        public int lastControlledFrames;
        
        public IActiveController activeController;

        public void TakeControl(Entity entity, IActiveController controller)
        {
            if (activeController != null && activeController != controller)
            {
#if UNITY_EDITOR
                Assert.IsTrue(CanInterrupt(entity, controller), $"{activeController.GetType().Name} cant be interrupted by {controller.GetType().Name}");
#endif
                activeController.OnInterrupt(entity, controller);
            }
            
            activeController = controller;
        }

        public void ReleaseControl(IActiveController controller)
        {
            if (activeController != controller)
            {
                return;
            }
            
            activeController = null;
        }

        public bool CanInterrupt(Entity entity, IActiveController controller = null)
        {
            if (activeController == null)
            {
                return true;
            }

            if (activeController == controller)
            {
                return false;
            }

            return activeController.CanBeInterrupted(entity, controller);
        }

        public bool IsControlled()
        {
            return activeController != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsControlled(IActiveController controller)
        {
            return activeController == controller;
        }
    }

    public struct KillCountComponent : IEntityComponent
    {
        public int count;
    }

    public struct CameraShakeProvider : IEntityComponent
    {
        public CameraShake shake;

        public void AddShake(CameraShake shake)
        {
            this.shake = shake;
        }
        
        public void AddShake(CameraShakeAsset shake)
        {
            if (shake != null)
            {
                AddShake(shake.shake);
            }
        }
    }

    public struct GroundComponent : IEntityComponent
    {
        
    }

    public struct HasLookingDirectionIndicatorComponent : IEntityComponent
    {
        public GameObject instance;
        public Transform pivot;
        public ModelComponent.Visiblity visiblity;
    }
    

}