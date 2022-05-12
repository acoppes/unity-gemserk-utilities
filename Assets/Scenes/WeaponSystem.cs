using System;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public struct Weapon
{
    public float cooldown;
}

public class WeaponSystem : BaseSystem, IEcsInitSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Init(EcsSystems systems)
    {
        
    }

    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Weapon>().End();
        var weapons = systems.GetWorld().GetPool<Weapon>();

        foreach (var entity in filter)
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.cooldown -= 1;
            Debug.Log(weapon.cooldown);

            if (weapon.cooldown < 0)
            {
                systems.GetWorld().DelEntity(entity);
            }
        }
    }
}