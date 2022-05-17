using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs.Extensions
{
    public class CreateEntitySystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<CreateEntity>().End();
            var createEntities = world.GetComponents<CreateEntity>();
            
            foreach (var entity in filter)
            {
                ref var createEntity = ref createEntities.Get(entity);
                
                createEntity.definition.Apply(world, entity);

                // EVENT ON ENTITY CREATED

                world.OnEntityCreated(entity);
                
                createEntities.Del(entity);
            }
        }
    }
}