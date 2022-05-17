using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public static class EcsSystemsExtensions
    {
        public static void AddComponent<T>(this EcsSystems systems, int entity) where T : struct
        {
            systems.GetWorld().GetPool<T>().Add(entity);
        }
        
        public static void AddComponent<T>(this EcsSystems systems, int entity, T t) where T : struct
        {
            ref var newT = ref systems.GetWorld().GetPool<T>().Add(entity);
            newT = t;
        }
        
        public static ref T GetComponent<T>(this EcsSystems systems, int entity) where T : struct
        {
            return ref systems.GetWorld().GetPool<T>().Get(entity);
        }

        public static void DelEntity(this EcsSystems systems, int entity)
        {
            systems.GetWorld().DelEntity(entity);
        }
    }
}