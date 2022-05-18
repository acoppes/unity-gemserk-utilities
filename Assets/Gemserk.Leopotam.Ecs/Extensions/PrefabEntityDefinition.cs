using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Extensions
{
    public class PrefabEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public void Apply(World world, int entity)
        {
            var templateParts = GetComponentsInChildren<IEntityDefinition>().ToList();
            templateParts.Remove(this);
            foreach (var template in templateParts)
            {
                template.Apply(world, entity);
            }
        }
    }
}