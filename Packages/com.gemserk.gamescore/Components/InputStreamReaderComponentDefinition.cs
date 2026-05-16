using System.IO;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct InputReaderComponent : IEntityComponent
    {
        public string path;
        public StreamReader reader;
    }
    
    public class InputStreamReaderComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string path;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new InputReaderComponent()
            {
                path = path
            });
        }
    }
}