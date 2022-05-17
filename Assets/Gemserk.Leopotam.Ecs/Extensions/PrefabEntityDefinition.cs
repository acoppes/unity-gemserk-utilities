using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Extensions
{
    public class PrefabEntityDefinition : MonoBehaviour, IEntityDefinition
    {
        public void Apply(World world, int entity)
        {
            var templateParts = GetComponentsInChildren<IEntityDefinition>();
            foreach (var template in templateParts)
            {
                if (template == this)
                    continue;
                template.Apply(world, entity);
            }
        }
    }
}