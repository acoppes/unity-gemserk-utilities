using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

public class ControllerExampleDefinition : MonoBehaviour, IEntityDefinition
{
    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, StatesComponent.Create());
    }
}
