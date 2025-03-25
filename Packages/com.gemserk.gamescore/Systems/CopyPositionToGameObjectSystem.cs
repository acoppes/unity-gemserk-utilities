using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Game.Systems
{
    public class CopyPositionToGameObjectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent, CopyPositionFromEntityComponent>, Exc<DisabledComponent>> fromEntity = default;
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent, CopyPositionFromGameObjectComponent>, Exc<DisabledComponent>> fromGameObject = default;

        // private struct SetPositionsToTransformsJob : IJobParallelForTransform
        // {
        //     [ReadOnly]
        //     public NativeArray<Vector3> positionsArray;
        //     
        //     public void Execute(int index, TransformAccess transform)
        //     {
        //         transform.position = positionsArray[index];
        //     }
        // }
        
        public void Run(EcsSystems systems)
        {
            // var entitiesCount = fromEntity.Value.GetEntitiesCount();
            //
            // if (entitiesCount > 0)
            // {
            //     var transformAccessArray = new TransformAccessArray(entitiesCount);
            //     var positionsArray = new NativeArray<Vector3>(entitiesCount, Allocator.Persistent);
            //
            //     var i = 0;

                foreach (var e in fromEntity.Value)
                {
                    ref var position = ref fromEntity.Pools.Inc1.Get(e);
                    ref var gameObjectComponent = ref fromEntity.Pools.Inc2.Get(e);

                    // transformAccessArray.Add(gameObjectComponent.transform);
                    
                    if (position.type == 0)
                    {
                        // positionsArray[i] = GamePerspective.ConvertFromWorld(position.value);
                        gameObjectComponent.gameObject.transform.position =
                            GamePerspective.ConvertFromWorld(position.value);
                    }
                    else if (position.type == 1)
                    {
                        // positionsArray[i] = position.value;
                        gameObjectComponent.gameObject.transform.position = position.value;
                    }

                    // i++;
                }

            //     var job = new SetPositionsToTransformsJob()
            //     {
            //         positionsArray = positionsArray
            //     };
            //
            //     // job.RunReadOnly(transformAccessArray);
            //     
            //     var jobHandle = job.Schedule(transformAccessArray);
            //     jobHandle.Complete();
            //
            //     transformAccessArray.Dispose();
            //     positionsArray.Dispose();
            // }
            
            foreach (var e in fromGameObject.Value)
            {
                ref var position = ref fromEntity.Pools.Inc1.Get(e);
                ref var gameObjectComponent = ref fromEntity.Pools.Inc2.Get(e);

                position.value = gameObjectComponent.gameObject.transform.position;
            }
            
        }
    }
}