using Gemserk.Leopotam.Ecs;
using UnityEngine;

public struct WeaponComponent : IEntityComponent
{
    public float cooldown;
    public GameObject gameObject;
    public EntityReference target;
}

public struct TargetComponent : IEntityComponent
{
    
}