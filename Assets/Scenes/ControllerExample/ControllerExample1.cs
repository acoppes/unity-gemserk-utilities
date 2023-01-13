using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;
using UnityEngine.Assertions;

public class ControllerExample1 : ControllerBase, IUpdate, IInit, IStateChanged
{
    public float myValue;
    public float testIncrement;

    private bool initialized;
    
    public void OnInit()
    {
        initialized = true;
    }
    
    public void OnUpdate(float dt)
    {
        var controllerComponent = Get<ControllerComponent>();
        var statesComponent = Get<StatesComponent>();
        
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


    public void OnEnterState()
    {
        var statesComponent = Get<StatesComponent>();
        Debug.Log($"ENTERED STATES: {string.Join(",", statesComponent.statesEntered)}");
    }

    public void OnExitState()
    {
        var statesComponent = Get<StatesComponent>();
        Debug.Log($"EXIT STATES: {string.Join(",", statesComponent.statesExited)}");
    }
}
