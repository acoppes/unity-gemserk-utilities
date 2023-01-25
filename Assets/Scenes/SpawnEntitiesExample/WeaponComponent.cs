using Gemserk.Leopotam.Ecs;

public struct WeaponComponent : IEntityComponent
{
    public float cooldown;
    public float currentCooldown;
    
    public Entity target;

    public IEntityDefinition bulletDefinition;
}

public struct TargetComponent : IEntityComponent
{
    
}