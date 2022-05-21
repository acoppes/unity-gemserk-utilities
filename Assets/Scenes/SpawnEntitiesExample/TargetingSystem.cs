using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

public class TargetingSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
{
    public void OnEntityDestroyed(World world, int entity)
    {
        var weaponsFilter = world.GetFilter<WeaponComponent>().End();
        var weaponComponents = world.GetComponents<WeaponComponent>();

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
        var filter = world.GetFilter<WeaponComponent>().Exc<DelayedDestroyComponent>().End();
        // var weapons = systems.GetWorld().GetPool<Weapon>();
        var weaponComponents = world.GetComponents<WeaponComponent>();
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