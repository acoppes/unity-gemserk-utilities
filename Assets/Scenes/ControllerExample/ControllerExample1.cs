using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

public class ControllerExample1 : ControllerBase
{
    public float myValue;
    public float testIncrement;
    
    public override void OnUpdate(float dt)
    {
        myValue += testIncrement * dt;

        if (myValue > 50)
        {
            world.DestroyEntity(entity);
        }
    }
}