using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class PrefabEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public void Apply(World world, int entity)
        {
            var subDefinitions = GetComponentsInChildren<IEntityDefinition>().ToList();
            subDefinitions.Remove(this);
            foreach (var definition in subDefinitions)
            {
                definition.Apply(world, entity);
            }
        }
    }
}