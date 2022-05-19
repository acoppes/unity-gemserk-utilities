using Gemserk.Leopotam.Ecs;
using UnityEngine;

public struct WeaponComponent : IEntityComponent
{
    public float cooldown;
    public float currentCooldown;
    
    public EntityReference target;

    public IEntityDefinition bulletDefinition;
}

public struct TargetComponent : IEntityComponent
{
    
}