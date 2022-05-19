using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem, IEcsInitSystem
{
    readonly EcsFilterInject<Inc<WeaponComponent>, Exc<DelayedDestroyComponent>> weaponsFilter = default;
    readonly EcsPoolInject<WeaponComponent> weaponComponents = default;
    
    public void Init(EcsSystems systems)
    {
        world.onEntityCreated += OnEntityCreated;
        world.onEntityDestroyed += OnEntityDestroyed;
    }
    
    private void OnEntityCreated(World world, int entity)
    {
        if (weaponComponents.Value.Has(entity))
        {
            ref var weapon = ref weaponComponents.Value.Get(entity);
            weapon.gameObject = new GameObject($"WEAPON_{entity}");
     
            var singletons = world.GetComponents<SingletonComponent>();
            if (singletons.Has(entity))
            {
                weapon.gameObject.name = $"WEAPON_{singletons.Get(entity).name}";
            }
        }
    }

    private void OnEntityDestroyed(World world, int entity)
    {
        if (weaponComponents.Value.Has(entity))
        {
            ref var weapon = ref weaponComponents.Value.Get(entity);
            Destroy(weapon.gameObject);
            weapon.gameObject = null;
        }
    }

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