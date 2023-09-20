using System.IO;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct InputRecorderComponent : IEntityComponent
    {
        public StreamWriter writer;
    }
    
    public class InputRecorderComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string path;
        
        public override string GetComponentName()
        {
            return nameof(InputRecorderComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new InputRecorderComponent()
            {
                writer = new StreamWriter(path, false)
            });
        }
    }
}