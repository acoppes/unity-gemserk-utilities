using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class ReferenceToGameObjectDefinition : MonoBehaviour, IEntityDefinition
{
    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new ReferenceToGameObjectComponent());
    }
}