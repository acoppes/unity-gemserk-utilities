using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class InputRecorderSystem : BaseSystem, IEcsRunSystem
    {
        // copy controls from unique input entity to each entity controlled by that control 
        readonly EcsFilterInject<Inc<InputComponent, InputRecorderComponent>, Exc<DisabledComponent>> filter = default;

        public InputComponentSerializer serializer = new InputComponentSerializer();
        
        public void Run(EcsSystems systems)
        {
            foreach (var e0 in filter.Value)
            {
                var input = filter.Pools.Inc1.Get(e0);
                var recorder = filter.Pools.Inc2.Get(e0);

                if (input.actions == null)
                {
                    continue;
                }
                
                serializer.Serialize(input, recorder.writer);
            }
        }
    }
}