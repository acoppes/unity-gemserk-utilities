using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

public class ControllerExample1 : ControllerBase, IUpdate
{
    public float myValue;
    public float testIncrement;
    
    public void OnUpdate(float dt)
    {
        myValue += testIncrement * dt;

        if (myValue > 50)
        {
            world.DestroyEntity(entity);
        }
    }
}
