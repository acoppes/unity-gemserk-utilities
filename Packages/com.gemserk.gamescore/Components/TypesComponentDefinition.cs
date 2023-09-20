using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct TypesComponent : IEntityComponent
    {
        public List<string> types;
    }
    
    public class TypesComponentDefinition : ComponentDefinitionBase
    {
        public List<string> types;
        public bool forceLowerCase = true;
        
        public override string GetComponentName()
        {
            return nameof(TypesComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            var typesComponent = new TypesComponent()
            {
                types = new List<string>()
            };

            if (forceLowerCase)
            {
                typesComponent.types.AddRange(types.Select(s => s.ToLower()).ToList());
            }
            else
            {
                typesComponent.types.AddRange(types);
            }
            
            world.AddComponent(entity, typesComponent);
        }
    }
}