using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs
{
    public class SingletonSystem : BaseSystem, IEcsInitSystem
    {
        readonly EcsPoolInject<SingletonComponent> singletons = default;
        
        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
            world.onEntityDestroyed += OnEntityDestroyed;
        }

        private void OnEntityCreated(World world, int entity)
        {
            var singletons = this.singletons.Value;
            
            if (!singletons.Has(entity))
            {
                return;
            }
            
            var singleton = singletons.Get(entity);

            var singletonEntities = world.sharedData.singletonEntities;
            
            if (singletonEntities.ContainsKey(singleton.name))
            {
                var oldEntity = singletonEntities[singleton.name];
                if (oldEntity != entity)
                {
                    throw new Exception($"Can't have two entities with same name {singleton.name}");
                }
            }
                
            singletonEntities[singleton.name] = entity;
        }

        private void OnEntityDestroyed(World world, int entity)
        {
            var singletons = this.singletons.Value;
            
            if (!singletons.Has(entity))
            {
                return;
            }
            
            var singletonEntities = world.sharedData.singletonEntities;
            
            ref var singleton = ref singletons.Get(entity);
            singletonEntities.Remove(singleton.name);
            singleton.name = null;
        }

    }
}