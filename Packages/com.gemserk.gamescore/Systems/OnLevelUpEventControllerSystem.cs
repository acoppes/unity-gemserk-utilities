using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public interface ILevelUpEvent : IControllerEvent
    {
        void OnLevelUp(World world, Entity entity);
    }
    
    public class OnLevelUpEventControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent, LevelComponent>, Exc<DisabledComponent>>
            filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var controllers = ref filter.Pools.Inc1.Get(e);
                ref var level = ref filter.Pools.Inc2.Get(e);

                if (!level.levelUpLastFrame)
                {
                    continue;
                }

                foreach (var controller in controllers.controllers)
                {
                    var levelUpEvent = controller as ILevelUpEvent;
                    levelUpEvent?.OnLevelUp(world, world.GetEntity(e));
                }
            }
        }
    }
}