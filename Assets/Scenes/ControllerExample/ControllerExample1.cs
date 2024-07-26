using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

public class ControllerExample1 : ControllerBase, IUpdate, IInit, IStateChanged
{
    public float myValue;
    public float testIncrement;

    public Cooldown cooldown;

    [InterfaceReferenceType]
    public InterfaceReference<IEntityDefinition> reference;

    [ObjectType(typeof(IEntityDefinition))]
    public Object entityDefinition;

    // [ObjectType()]
    // public Object entityDefinition;
    
    private bool initialized;
    
    public void OnInit(World world, Entity entity)
    {
        initialized = true;

        myValue = 0;

        var def = reference.Get();
        Debug.Log(def == null);

        // var gameObject = UnityObjectExtensions.GetGameObjectFromInterface(def);

        // var gameObject = reference.AsGameObject();
        // Debug.Log(gameObject.name);
    }
    
    public void OnUpdate(World world, Entity entity, float dt)
    {
        
        var controllerComponent = world.GetComponent<ControllerComponent>(entity);
        var statesComponent = world.GetComponent<StatesComponent>(entity);
        
        Assert.IsNotNull(controllerComponent.instance);
        Assert.IsTrue(initialized, "Init should be called always before update");
        
        myValue += testIncrement * dt;

        if (myValue > 50)
        {
            world.DestroyEntity(entity);
        }

        if (statesComponent.TryGetState("State1", out var state))
        {
            Debug.Log(state.time);
            Debug.Log(state.updateCount);
            
            statesComponent.ExitState("State1");
        }
    }


    public void OnEnterState(World world, Entity entity)
    {
        var statesComponent = entity.Get<StatesComponent>();
        Debug.Log($"ENTERED STATES: {string.Join(",", statesComponent.statesEntered)}");
    }

    public void OnExitState(World world, Entity entity)
    {
        var statesComponent = entity.Get<StatesComponent>();
        Debug.Log($"EXIT STATES: {string.Join(",", statesComponent.statesExited)}");
    }
}
