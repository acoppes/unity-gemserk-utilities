﻿using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class TimeSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TimeComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            foreach (var entity in filter.Value)
            {
                ref var time = ref filter.Pools.Inc1.Get(entity);
                time.time += deltaTime;
            }
        }
    }
}