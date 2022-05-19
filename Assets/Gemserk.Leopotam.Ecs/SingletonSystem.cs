using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs
{
    public class SingletonSystem : BaseSystem, IEcsInitSystem
    {
        readonly EcsPoolInject<NameComponent> names = default;
        
        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
            world.onEntityDestroyed += OnEntityDestroyed;
        }

        private void OnEntityCreated(World world, int entity)
        {
            var names = this.names.Value;
            
            if (!names.Has(entity))
            {
                return;
            }
            
            var nameComponent = names.Get(entity);

            if (!nameComponent.singleton)
                return;

            var singletonByNameEntities = world.sharedData.singletonByNameEntities;
            
            if (singletonByNameEntities.ContainsKey(nameComponent.name))
            {
                var oldEntity = singletonByNameEntities[nameComponent.name];
                if (oldEntity != entity)
                {
                    throw new Exception($"Can't have two entities with same name {nameComponent.name}");
                }
            }
                
            singletonByNameEntities[nameComponent.name] = entity;
        }

        private void OnEntityDestroyed(World world, int entity)
        {
            var names = this.names.Value;
            
            if (!names.Has(entity))
            {
                return;
            }
            
            var singletonByNameEntities = world.sharedData.singletonByNameEntities;
            
            ref var nameComponent = ref names.Get(entity);
            singletonByNameEntities.Remove(nameComponent.name);
            nameComponent.name = null;
            nameComponent.singleton = false;
        }

    }
}