using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    // this class is optional if we want to have an instance parameter that is not a component
    // definition base. Another option could be to implement something extra when filtering
    // could be an attribute to define if a component can be used as parameter and/or as component.
    // and then filter by that in the editor.
    public abstract class EntityInstanceParameterBase : MonoBehaviour, IEntityInstanceParameter
    {
        public abstract void Apply(World world, Entity entity);
    }
}