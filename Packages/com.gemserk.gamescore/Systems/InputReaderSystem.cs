using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class InputReaderSystem : BaseSystem, IEcsRunSystem
    {
        // copy controls from unique input entity to each entity controlled by that control 
        readonly EcsFilterInject<Inc<InputComponent, InputReaderComponent>, Exc<DisabledComponent>> filter = default;

        public InputComponentSerializer serializer = new InputComponentSerializer();
        
        public void Run(EcsSystems systems)
        {
            foreach (var e0 in filter.Value)
            {
                ref var input = ref filter.Pools.Inc1.Get(e0);
                ref var reader = ref filter.Pools.Inc2.Get(e0);

                if (reader.reader == null)
                {
                    continue;
                }
                
                serializer.Deserialize(ref input, reader.reader);
            }
        }
    }
}