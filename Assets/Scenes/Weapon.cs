using Gemserk.Leopotam.Ecs;

public struct Weapon : IEntityComponent
{
    public string name;
    public float cooldown;
}

public struct ToDestroy : IEntityComponent
{
    public int val;
    public int val2;
}