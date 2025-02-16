using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Systems
{
    public class PhysicsVelocityDebugSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Physics2dComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public void Run(EcsSystems systems)
        {
            #if UNITY_EDITOR
            
            foreach (var entity in physics2dFilter.Value)
            {
                var physics2dComponent = physics2dFilter.Pools.Inc1.Get(entity);
                
                if (physics2dComponent.body == null)
                {
                    continue;
                }

                var body = physics2dComponent.body;
                D.raw(new Shape.Line(
                    body.position + new Vector2(0, 0.5f), 
                    body.position + physics2dComponent.velocity + new Vector2(0, 0.5f)), 
                    Color.green);
            }
            #endif
        }
    }
}