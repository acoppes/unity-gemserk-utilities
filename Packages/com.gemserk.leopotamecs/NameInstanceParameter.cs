using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class NameInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public string entityName;
        public bool singleton;
        
        public void Apply(World world, Entity entity)
        {
            if (!entity.Has<NameComponent>())
            {
                world.AddComponent(entity, new NameComponent());
            }
            ref var name = ref entity.Get<NameComponent>();
            name.name = entityName;
            name.singleton = singleton;
        }
    }
}