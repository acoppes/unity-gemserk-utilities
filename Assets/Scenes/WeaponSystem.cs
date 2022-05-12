using System;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Weapon>().End();
        var weapons = systems.GetWorld().GetPool<Weapon>();

        // var sceneController = systems.GetShared<SampleSceneController>();
        // Debug.Log(sceneController.test);
        
        foreach (var entity in filter)
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.cooldown -= Time.deltaTime;
            Debug.Log(weapon.cooldown);

            if (weapon.cooldown < 0)
            {
                systems.GetWorld().DelEntity(entity);
            }
        }
    }
}