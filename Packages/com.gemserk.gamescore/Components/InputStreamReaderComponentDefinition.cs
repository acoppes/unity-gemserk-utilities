using System.IO;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct InputReaderComponent : IEntityComponent
    {
        public StreamReader reader;
    }
    
    public class InputStreamReaderComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string path;
        
        public override string GetComponentName()
        {
            return nameof(InputReaderComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new InputReaderComponent()
            {
                reader = new StreamReader(new FileStream(path, FileMode.Open))
            });
        }
    }
}