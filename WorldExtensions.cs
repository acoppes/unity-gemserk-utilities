using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public static class WorldExtensions 
    {
        public static void AddComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            world.GetPool<T>().Add(entity);
        }
        
        public static void AddComponent<T>(this EcsWorld world, int entity, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(entity);
            newT = t;
        }
        
        public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }
    }
}