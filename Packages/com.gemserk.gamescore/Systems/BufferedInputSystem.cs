using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace Game.Systems
{
    public class BufferedInputSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<BufferedInputComponent>> bufferInputFilter = default;
        readonly EcsFilterInject<Inc<InputComponent, BufferedInputComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            foreach (var entity in bufferInputFilter.Value)
            {
                ref var bufferedInput = ref bufferInputFilter.Pools.Inc1.Get(entity);
                bufferedInput.currentBufferTime -= deltaTime;
            }
            
            foreach (var e in filter.Value)
            {
                ref var controlComponent = ref filter.Pools.Inc1.Get(e);
                ref var bufferedInputComponent = ref filter.Pools.Inc2.Get(e);
                
                foreach (var actionName in controlComponent.actions.Keys)
                {
                    var action = controlComponent.actions[actionName];
                    if (action.type == InputActionType.Button)
                    {
                        if (action.wasPressed)
                        {
                            bufferedInputComponent.InsertInBuffer(actionName);
                        }
                    }
                }
            }
        }
    }
}