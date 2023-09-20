using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ClearControlsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<BufferedInputComponent>> bufferInputFilter = default;
        readonly EcsFilterInject<Inc<InputComponent>> controlsFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in controlsFilter.Value)
            {
                ref var controlComponent = ref controlsFilter.Pools.Inc1.Get(entity);
                controlComponent.ClearPressedChanged();
            }
            
            foreach (var entity in bufferInputFilter.Value)
            {
                ref var bufferedInput = ref bufferInputFilter.Pools.Inc1.Get(entity);
                if (bufferedInput.currentBufferTime < 0 && bufferedInput.buffer.Count > 0)
                {
                    bufferedInput.buffer.Clear();
                }
            }
        }
    }
}