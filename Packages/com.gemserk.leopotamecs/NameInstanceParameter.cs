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
            ref var nameComponent = ref entity.Get<NameComponent>();
            nameComponent.name = entityName;
            nameComponent.singleton = singleton;
        }
    }
}