using System.IO;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class InputRecorderSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        // copy controls from unique input entity to each entity controlled by that control 
        readonly EcsFilterInject<Inc<InputComponent, InputRecorderComponent>, Exc<DisabledComponent>> filter = default;

        private readonly InputComponentSerializer serializer = new InputComponentSerializer();
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<InputRecorderComponent>())
            {
                ref var inputRecorder = ref entity.Get<InputRecorderComponent>();
                
                inputRecorder.writer?.Close();
                inputRecorder.writer = null;
                inputRecorder.writer = new StreamWriter(Path.Combine(Application.streamingAssetsPath, inputRecorder.path), false);
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (entity.Has<InputRecorderComponent>())
            {
                ref var inputRecorder = ref entity.Get<InputRecorderComponent>();
                inputRecorder.writer?.Close();
                inputRecorder.writer = null;
            }
        }
        
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