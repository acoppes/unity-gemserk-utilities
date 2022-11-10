using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class ControllerExample1 : ControllerBase
{
    public float myValue;
    public float testIncrement;
    
    public override void OnUpdate(float dt)
    {
        myValue += testIncrement * dt;
    }
}
