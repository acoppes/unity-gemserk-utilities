using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs
{
    public class SingletonByNameSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsPoolInject<NameComponent> names = default;

        public void OnEntityCreated(World world, Entity entity)
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

            nameComponent.cachedInSingletonsDictionary = true;
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            var names = this.names.Value;
            
            if (!names.Has(entity))
            {
                return;
            }
            
            ref var nameComponent = ref names.Get(entity);
            
            if (nameComponent.singleton) 
            {
                var singletonByNameEntities = world.sharedData.singletonByNameEntities;
                singletonByNameEntities.Remove(nameComponent.name);
            }
            
            nameComponent.name = null;
            nameComponent.singleton = false;
            
            nameComponent.cachedInSingletonsDictionary = false;
        }

        public void Run(EcsSystems systems)
        {
            var nameComponents = world.GetComponents<NameComponent>();
            foreach (var entity in world.GetFilter<NameComponent>().End())
            {
                ref var nameComponent = ref nameComponents.Get(entity);
                
                // Having a singleton name component to have a null or empty name should be an error. 
                if (string.IsNullOrEmpty(nameComponent.name))
                {
                    continue;
                }

                if (!nameComponent.cachedInSingletonsDictionary && nameComponent.singleton)
                {
                    var singletonByNameEntities = world.sharedData.singletonByNameEntities;
            
                    if (singletonByNameEntities.ContainsKey(nameComponent.name))
                    {
                        var oldEntity = singletonByNameEntities[nameComponent.name];
                        if (oldEntity != entity)
                        {
                            throw new Exception($"Can't have two entities with same name {nameComponent.name}");
                        }
                    }
                    singletonByNameEntities[nameComponent.name] = this.GetEntity(entity);
                    nameComponent.cachedInSingletonsDictionary = true;
                }
            }
        }
    }
}