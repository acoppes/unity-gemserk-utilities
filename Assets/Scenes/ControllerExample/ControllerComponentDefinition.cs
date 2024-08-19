using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class ControllerComponentDefinition : ComponentDefinitionBase
{
    public GameObject controllerPrefab;
    public bool sharedInstance;
    
    public override void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new ControllerComponent()
        {
            prefab = controllerPrefab,
            sharedInstance = sharedInstance
        });
    }
}