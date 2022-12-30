using System;
using Gemserk.Leopotam.Ecs;

[Serializable]
public class MovementDefinition : EntityComponentDefinitionBase
{
    public bool speed;
    
    public void Apply(World world, Entity entity)
    {
        
    }
}