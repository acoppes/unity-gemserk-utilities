using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class CopyInputToMovementSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputComponent, MovementComponent, CopyInputToMovementComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var input = filter.Pools.Inc1.Get(e);
                ref var movement = ref filter.Pools.Inc2.Get(e);
                var copyMovement = filter.Pools.Inc3.Get(e);
                
                // TODO: maybe disable this with a config or something?
                movement.movingDirection = input.direction3d();

                if (copyMovement.fixedAngles > 0 && input.direction().vector2.SqrMagnitude() > 0)
                {
                    var direction = input.direction().vector2.FixToAngles(copyMovement.fixedAngles);
                    movement.movingDirection = new Vector3(direction.x, 0, direction.y);
                }
            }
            
        }
    }
}