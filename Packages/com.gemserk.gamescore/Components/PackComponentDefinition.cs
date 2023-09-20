using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct PackComponent : IEntityComponent
    {
        public List<IEntityDefinition> definitions;
    }
    
    public class PackComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(PackComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PackComponent()
            {
                definitions = new List<IEntityDefinition>()
            });
        }
    }
}