using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class ToDestroySystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<ToDestroy>().End();
        
        foreach (var entity in filter)
        {
            var toDestroy = systems.GetComponent<ToDestroy>(entity);
            Debug.Log($"{toDestroy.val}, {toDestroy.val2}");
            systems.DelEntity(entity);
        }
    }
}