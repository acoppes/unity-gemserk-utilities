using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class SpawnPackSystem : BaseSystem, IEcsRunSystem
    {
        public SignalAsset onSpawnPackCompletedSignal;
        
        readonly EcsFilterInject<Inc<SelectionComponent, SpawnPackComponent>, Exc<DisabledComponent>> spawnPackFilter = default;
        readonly EcsFilterInject<Inc<SpawnPackComponent, DestroyableComponent>, Exc<DisabledComponent>> filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in spawnPackFilter.Value)
            {
                var selection = spawnPackFilter.Pools.Inc1.Get(e);
                ref var spawnPack = ref spawnPackFilter.Pools.Inc2.Get(e);

                spawnPack.unitsCount = 0;
                foreach (var selectedEntity in selection.selectedEntities)
                {
                    if (selectedEntity.Exists())
                    {
                        spawnPack.unitsCount++;
                    }
                }
            }
            
            foreach (var e in filter.Value)
            {
                var spawnPack = filter.Pools.Inc1.Get(e);
                ref var destroyable = ref filter.Pools.Inc2.Get(e);

                if (spawnPack.unitsCount == 0)
                {
                    // signal spawn pack destroyed...
                    if (onSpawnPackCompletedSignal != null)
                    {
                        onSpawnPackCompletedSignal.Signal(world.GetEntity(e));
                    }
                    destroyable.destroy = true;
                }
            }
        }
    }
}