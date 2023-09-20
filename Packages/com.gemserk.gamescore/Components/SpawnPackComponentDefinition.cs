using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct SpawnPackComponent : IEntityComponent
    {
        // auto destroys if all entities destroyed?

        public SpawnPackData spawnPackData;
        public List<IEntityDefinition> spawnPackDefinitions => spawnPackData.definitions;

        public int currentSpawnIndex;
        public int unitsCount;
        
        public bool spawnPackCompleted => currentSpawnIndex >= spawnPackDefinitions.Count;
    }
    
    public class SpawnPackComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(SpawnPackComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new SpawnPackComponent());
        }
    }
}