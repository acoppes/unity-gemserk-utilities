using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class AutoDestroyEntitiesMaxByTypeSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MaxByTypeComponent, DestroyableComponent>, Exc<DisabledComponent>> filter = default;

        private readonly IDictionary<ITypeMax, int> countPerType = new Dictionary<ITypeMax, int>();

        private readonly List<int> entitiesToCheck = new List<int>();
        
        public void Run(EcsSystems systems)
        {
            countPerType.Clear();
            entitiesToCheck.Clear();
            
            foreach (var entity in filter.Value)
            {
                ref var maxByType = ref filter.Pools.Inc1.Get(entity);
                if (!countPerType.ContainsKey(maxByType.type))
                {
                    countPerType[maxByType.type] = 0;
                }
                countPerType[maxByType.type]++;
            }
            
            foreach (var entity in filter.Value)
            {
                ref var maxByType = ref filter.Pools.Inc1.Get(entity);

                if (countPerType[maxByType.type] > maxByType.type.GetMax())
                {
                    entitiesToCheck.Add(entity);
                }
                
                maxByType.updates++;
            }

            if (entitiesToCheck.Count > 0)
            {
                entitiesToCheck.Sort(delegate(int e1, int e2)
                {
                    var m1 = filter.Pools.Inc1.Get(e1);
                    var m2 = filter.Pools.Inc1.Get(e2);
                    
                    return m2.updates - m1.updates;
                });
                
                foreach (var e in entitiesToCheck)
                {
                    var maxByType = filter.Pools.Inc1.Get(e);
                    ref var destroyable = ref filter.Pools.Inc2.Get(e);
                    destroyable.destroy = destroyable.destroy || countPerType[maxByType.type] > maxByType.type.GetMax();
                    countPerType[maxByType.type]--;
                }
            }
        }
    }
}