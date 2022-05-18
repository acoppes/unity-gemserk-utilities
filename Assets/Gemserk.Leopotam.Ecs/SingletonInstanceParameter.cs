using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class SingletonInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public string entityName;
        
        public void Apply(World world, int entity)
        {
            world.AddComponent(entity, new SingletonComponent
            {
                name = entityName
            });
        }
    }
}