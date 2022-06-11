using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class NameInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public string entityName;
        public bool singleton;
        
        public void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new NameComponent
            {
                name = entityName,
                singleton = singleton
            });
        }
    }
}