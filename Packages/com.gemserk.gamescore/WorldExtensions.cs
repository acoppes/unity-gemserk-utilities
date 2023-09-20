using System;
using Gemserk.Leopotam.Ecs;

namespace Game
{
    public static class WorldExtensions
    {
        // TODO: move these utils to core
        
        public static T GetSingleton<T>(this World world) where T : struct
        {
            foreach (var entity in world.Filter<T>())
            {
                return world.GetComponents<T>().Get(entity);
            }
            throw new Exception("Couldn't find singleton entity with specified component.");
        } 
        
        public static bool TryGetSingletonComponent<T>(this World world, out T t) where T : struct
        {
            t = default;
            
            foreach (var entity in world.Filter<T>())
            {
                t = world.GetComponents<T>().Get(entity);
                return true;
            }
            
            return false;
        } 
        
        public static bool TryGetSingletonEntity<T1>(this World world, out Entity e) 
            where T1: struct 
        {
            e = Entity.NullEntity;
            
            foreach (var entity in world.Filter<T1>())
            {
                e = world.GetEntity(entity);
                return true;
            }
            
            return false;
        } 
        
        public static bool TryGetSingletonEntity<T1, T2>(this World world, out Entity e) 
            where T1: struct 
            where T2: struct
        {
            e = Entity.NullEntity;
            
            foreach (var entity in world.Filter<T1, T2>())
            {
                e = world.GetEntity(entity);
                return true;
            }
            
            return false;
        } 
    }
}