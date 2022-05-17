using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public static class EcsSystemsExtensions
    {
        public static void AddComponent<T>(this EcsSystems systems, int entity) where T : struct
        {
            systems.GetWorld().AddComponent<T>(entity);
        }
        
        public static void AddComponent<T>(this EcsSystems systems, int entity, T t) where T : struct
        {
            systems.GetWorld().AddComponent(entity, t);
        }
        
        public static ref T GetComponent<T>(this EcsSystems systems, int entity) where T : struct
        {
            return ref systems.GetWorld().GetComponent<T>(entity);
        }

        public static void DelEntity(this EcsSystems systems, int entity)
        {
            systems.GetWorld().DelEntity(entity);
        }
    }
}