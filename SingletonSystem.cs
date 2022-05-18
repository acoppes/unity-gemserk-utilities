using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public class SingletonSystem : BaseSystem, IFixedUpdateSystem, IEcsInitSystem
    {
        // TODO: dictionary should be easily accesible from outside
        private readonly IDictionary<string, int> singletonDictionary = new Dictionary<string, int>();

        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
            world.onEntityDestroyed += OnEntityDestroyed;
        }

        private void OnEntityCreated(World world, int entity)
        {
            var singletons = world.GetComponents<SingletonComponent>();
            
            if (!singletons.Has(entity))
            {
                return;
            }
            
            var singleton = singletons.Get(entity);
                
            if (singletonDictionary.ContainsKey(singleton.name))
            {
                var oldEntity = singletonDictionary[singleton.name];
                if (oldEntity != entity)
                {
                    throw new Exception($"Can't have two entities with same name {singleton.name}");
                }
            }
                
            singletonDictionary[singleton.name] = entity;
        }

        private void OnEntityDestroyed(World world, int entity)
        {
            var singletons = world.GetComponents<SingletonComponent>();
            
            if (!singletons.Has(entity))
            {
                return;
            }
            
            ref var singleton = ref singletons.Get(entity);
            singletonDictionary.Remove(singleton.name);
            singleton.name = null;
        }

        // public void Run(EcsSystems systems)
        // {
        //     var filter = world.GetFilter<SingletonComponent>().End();
        //     var singletons = world.GetComponents<SingletonComponent>();
        //
        //     foreach (var entity in filter)
        //     {
        //         var singleton = singletons.Get(entity);
        //         if (singletonDictionary.ContainsKey(singleton.name))
        //         {
        //             var oldEntity = singletonDictionary[singleton.name];
        //             if (oldEntity != entity)
        //             {
        //                 throw new Exception($"Can't have two entities with same name {singleton.name}");
        //             }
        //         }
        //         singletonDictionary[singleton.name] = entity;
        //     }
        // }

    }
}