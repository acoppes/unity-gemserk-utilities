using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.Assertions;

public class ControllerExample1 : ControllerBase, IUpdate
{
    public float myValue;
    public float testIncrement;
    
    public void OnUpdate(float dt)
    {
        var controllerComponent = GetComponent<ControllerComponent>();
        
        Assert.IsNotNull(controllerComponent.instance);
        
        myValue += testIncrement * dt;

        if (myValue > 50)
        {
            world.DestroyEntity(entity);
        }
    }
}
