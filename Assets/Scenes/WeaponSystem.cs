using System;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsInitSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Init(EcsSystems systems)
    {
        
    }

    public void Run(EcsSystems systems)
    {
        Debug.Log("Fixed update!");
    }
}