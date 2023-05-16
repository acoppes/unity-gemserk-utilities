using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class StatesDefinition : MonoBehaviour, IEntityDefinition
{
    public List<string> startingStates = new List<string>();

    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, StatesComponent.Create());

        ref var statesComponent = ref world.GetComponent<StatesComponent>(entity);

        foreach (var state in startingStates)
        {
            statesComponent.EnterState(state);
        }
    }
}