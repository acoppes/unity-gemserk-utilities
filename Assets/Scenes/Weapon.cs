using Gemserk.Leopotam.Ecs;
using UnityEngine;

public struct Weapon : IEntityComponent
{
    public string name;
    public float cooldown;
    public GameObject gameObject;
}

public struct ToDestroy : IEntityComponent
{
    public int val;
    public int val2;
}