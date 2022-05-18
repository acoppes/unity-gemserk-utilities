using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem, IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        world.onEntityCreated += OnEntityCreated;
        world.onEntityDestroyed += OnEntityDestroyed;
    }
    
    private static void OnEntityCreated(World world, int entity)
    {
        var weapons = world.GetComponents<Weapon>();
        var singletons = world.GetComponents<SingletonComponent>();
        if (weapons.Has(entity))
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.gameObject = new GameObject($"WEAPON_{entity}");
            if (singletons.Has(entity))
            {
                weapon.gameObject.name = $"WEAPON_{singletons.Get(entity).name}";
            }
        }
    }

    private static void OnEntityDestroyed(World world, int entity)
    {
        var weapons = world.GetComponents<Weapon>();
        if (weapons.Has(entity))
        {
            ref var weapon = ref weapons.Get(entity);
            Destroy(weapon.gameObject);
            weapon.gameObject = null;
        }
    }

    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Weapon>().Exc<ToDestroy>().End();
        var weapons = systems.GetWorld().GetPool<Weapon>();

        // var sceneController = systems.GetShared<SampleSceneController>();
        // Debug.Log(sceneController.test);
        
        foreach (var entity in filter)
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.cooldown -= Time.deltaTime;
            Debug.Log($"{GetType().Name}, FRAME: {Time.frameCount}, {weapon.cooldown}, target:{weapon.target}");

            if (weapon.cooldown < 0)
            {
                world.AddComponent(entity, new ToDestroy
                {
                    val = 10,
                    val2 = 20
                });
                // systems.GetWorld().GetPool<ToDestroy>().Add(entity);
                // systems.GetWorld().DelEntity(entity);
            }
        }
    }

}