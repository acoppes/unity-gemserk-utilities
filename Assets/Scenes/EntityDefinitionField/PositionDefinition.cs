using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

[Serializable]
public class PositionDefinition : EntityComponentDefinitionBase
{
    public Vector3 value;
    
    public void Apply(World world, Entity entity)
    {
        
    }
}