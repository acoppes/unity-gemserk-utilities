using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

public class WeaponSystem2 : BaseSystem, IEcsRunSystem, IFixedUpdateSystem, IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        world.onEntityDestroyed += OnEntityDestroyed;
    }
    
    private void OnEntityDestroyed(World world, int entity)
    {
        var weaponsFilter = world.GetFilter<Weapon>().End();
        var weaponComponents = world.GetComponents<Weapon>();

        foreach (var weapon in weaponsFilter)
        {
            ref var weaponComponent = ref weaponComponents.Get(weapon);

            if (weaponComponent.target == entity)
            {
                weaponComponent.target.SetNull();
            }
        }
    }
    
    public void Run(EcsSystems systems)
    {
        var filter = world.GetFilter<Weapon>().Exc<ToDestroy>().End();
        // var weapons = systems.GetWorld().GetPool<Weapon>();
        var weaponComponents = world.GetComponents<Weapon>();
        // var targetComponents = world.GetComponents<TargetComponent>();

        // var sceneController = systems.GetShared<SampleSceneController>();
        // Debug.Log(sceneController.test);
        
        foreach (var weapon in filter)
        {
            ref var weaponComponent = ref weaponComponents.Get(weapon);
            
            foreach (var target in world.GetFilter<TargetComponent>().End())
            {
                // ref var targetComponent = ref targetComponents.Get(target);
                if (weapon != target)
                {
                    weaponComponent.target = target;
                }
            }
            
        }
    }
}