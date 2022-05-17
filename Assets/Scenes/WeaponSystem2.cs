using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem2 : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Weapon>().Exc<ToDestroy>().End();
        var weapons = systems.GetWorld().GetPool<Weapon>();

        // var sceneController = systems.GetShared<SampleSceneController>();
        // Debug.Log(sceneController.test);
        
        foreach (var entity in filter)
        {
            ref var weapon = ref weapons.Get(entity);
            Debug.Log($"{GetType().Name}, FRAME: {Time.frameCount}, {weapon.name}");
        }
    }
}