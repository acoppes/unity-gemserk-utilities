using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct TypesComponent : IEntityComponent
    {
        public List<string> types;
    }
    
    public class TypesComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public List<string> types;
        public bool forceLowerCase = true;

        public override void Apply(World world, Entity entity)
        {
            if (!entity.Has<TypesComponent>())
            {
                world.AddComponent(entity, new TypesComponent()
                {
                    types = new List<string>()
                });
            }

            ref var typesComponent = ref entity.Get<TypesComponent>();

            if (forceLowerCase)
            {
                typesComponent.types.AddRange(types.Select(s => s.ToLower()).ToList());
            }
            else
            {
                typesComponent.types.AddRange(types);
            }
        }
    }
}