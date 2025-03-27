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
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent, CopyPositionFromEntityComponent>, 
            Exc<DisabledComponent, CopyPositionFromEntityStaticProcessedComponent>> fromEntity = default;
        
        readonly EcsFilterInject<Inc<CopyPositionFromEntityComponent, StaticObjectComponent>, 
            Exc<DisabledComponent, CopyPositionFromEntityStaticProcessedComponent>> fromEntityStatic = default;
        
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

            
            foreach (var e in fromGameObject.Value)
            {
                ref var position = ref fromEntity.Pools.Inc1.Get(e);
                ref var gameObjectComponent = ref fromEntity.Pools.Inc2.Get(e);

                position.value = gameObjectComponent.gameObject.transform.position;
            }
            
            foreach (var e in fromEntityStatic.Value)
            {
                world.AddComponent(e, new CopyPositionFromEntityStaticProcessedComponent());
            }
            
            // could add in the future something like, if no static and copy processed, then remove it, for the case
            // an object becomes non static in runtime.
        }
    }
}