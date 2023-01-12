using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class TargetingSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        public void OnEntityDestroyed(World world, Entity destroyedEntity)
        {
            if (world.HasComponent<TargetComponent>(destroyedEntity))
            {
                var targetComponent = world.GetComponent<TargetComponent>(destroyedEntity);
                
                var abilitiesComponents = world.GetComponents<AbilitiesComponent>();
                
                foreach (var entity in world.GetFilter<AbilitiesComponent>().End())
                {
                    ref var abilitiesComponent = ref abilitiesComponents.Get(entity);

                    foreach (var targeting in abilitiesComponent.targetings)
                    {
                        targeting.targets.Remove(targetComponent.target);
                    }
                }
            }
        }
            
        public void Run(EcsSystems systems)
        {
            var abilitiesComponents = world.GetComponents<AbilitiesComponent>();
            var playerComponents = world.GetComponents<PlayerComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();

            foreach (var entity in world.GetFilter<AbilitiesComponent>().Inc<PlayerComponent>().End())
            {
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);

                foreach (var targeting in abilitiesComponent.targetings)
                {
                    ref var parameters = ref targeting.parameters;
                    parameters.player = playerComponents.Get(entity).player;
                }
            }
            
            foreach (var entity in world.GetFilter<AbilitiesComponent>().Inc<PositionComponent>().End())
            {
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);

                foreach (var targeting in abilitiesComponent.targetings)
                {
                    ref var parameters = ref targeting.parameters;
                    parameters.position = positionComponents.Get(entity).value;
                }
            }
            
            foreach (var entity in world.GetFilter<AbilitiesComponent>().End())
            {
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);

                foreach (var targeting in abilitiesComponent.targetings)
                {
                    targeting.targets.Clear();
                    TargetingUtils.FindTargets(world, targeting.parameters, targeting.targets);
                }
            }
        }
    }
}