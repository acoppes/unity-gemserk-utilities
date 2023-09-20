using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Components
{
    public struct SpawnerComponent : IEntityComponent
    {
        // optional
        public IEntityDefinition spawnPackDefinition;
        
        public Cooldown cooldown;
        public List<SpawnPackData> pending;
        public Object area;

        public Entity lastFrameSpawnedEntity;
        public Entity currentSpawnPack;
        
        public float randomRadius;

        public bool paused;

        public static SpawnerComponent Create()
        {
            return new SpawnerComponent()
            {
                pending = new List<SpawnPackData>()
            };
        }
    }
    
    public class SpawnerComponentDefinition : ComponentDefinitionBase
    {
        public Object spawnPackDefinition;
        public float cooldown;
        public float randomRadius;

        public override string GetComponentName()
        {
            return nameof(SpawnerComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            var spawnerComponent = SpawnerComponent.Create();
            spawnerComponent.cooldown = new Cooldown(cooldown);
            spawnerComponent.currentSpawnPack = Entity.NullEntity;
            spawnerComponent.randomRadius = randomRadius;

            if (spawnPackDefinition != null)
            {
                spawnerComponent.spawnPackDefinition = spawnPackDefinition.GetInterface<IEntityDefinition>();
            }
            
            world.AddComponent(entity, spawnerComponent);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, randomRadius);
        }
    }
}