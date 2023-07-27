using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class ControllerExampleDefinition : MonoBehaviour, IEntityDefinition
{
    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, StatesComponent.Create());
    }
}
