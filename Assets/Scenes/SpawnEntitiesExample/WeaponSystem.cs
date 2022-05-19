using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    readonly EcsFilterInject<Inc<WeaponComponent>, Exc<DelayedDestroyComponent>> weaponsFilter = default;
    readonly EcsPoolInject<WeaponComponent> weaponComponents = default;

    public void Run(EcsSystems systems)
    {
        foreach (var entity in weaponsFilter.Value)
        {
            ref var weapon = ref weaponComponents.Value.Get(entity);
            weapon.currentCooldown += Time.deltaTime;

            if (weapon.currentCooldown > weapon.cooldown)
            {
                var bulletEntity = world.CreateEntity(weapon.bulletDefinition);
                
                // get bullet component and set start position and direction

                weapon.currentCooldown -= weapon.cooldown;
            }
        }
    }

}