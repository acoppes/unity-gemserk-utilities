using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class EndlessController : ControllerBase, IUpdate
    {
        public float maxFallingTime = 1.5f;
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var healthComponent = ref entity.GetHealthComponent();
            
            if (healthComponent.aliveType != HealthComponent.AliveType.Alive)
            {
                return;
            }
            
            ref var control = ref entity.Get<InputComponent>();
            
            control.left().isPressed = false;
            control.right().isPressed = true;
            control.direction().vector2 = Vector2.right;

            ref var platformerComponent = ref entity.GetPlatformerComponent();
            
            if (platformerComponent.fallingTime > maxFallingTime)
            {
                healthComponent.damages.Add(new DamageData()
                {
                    knockback = false,
                    position = Vector3.zero,
                    source = entity,
                    value = healthComponent.current
                });

                platformerComponent.fallingTime = 0;
            }
        }
    }
}