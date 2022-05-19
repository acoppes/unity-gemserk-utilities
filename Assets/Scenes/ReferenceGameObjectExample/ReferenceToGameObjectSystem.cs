using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class ReferenceToGameObjectSystem : BaseSystem, IEcsInitSystem
{
    readonly EcsPoolInject<ReferenceToGameObjectComponent> components = default;
    
    public void Init(EcsSystems systems)
    {
        world.onEntityCreated += OnEntityCreated;
        world.onEntityDestroyed += OnEntityDestroyed;
    }
    
    private void OnEntityCreated(World world, int entity)
    {
        if (components.Value.Has(entity))
        {
            ref var referenceToGameObjectComponent = ref components.Value.Get(entity);
            referenceToGameObjectComponent.gameObject = new GameObject($"GAMEOBJECT_{entity}");
     
            var singletons = world.GetComponents<SingletonComponent>();
            if (singletons.Has(entity))
            {
                referenceToGameObjectComponent.gameObject.name = $"GAMEOBJECT_{singletons.Get(entity).name}";
            }
        }
    }

    private void OnEntityDestroyed(World world, int entity)
    {
        if (components.Value.Has(entity))
        {
            ref var referenceToGameObjectComponent = ref components.Value.Get(entity);
            Destroy(referenceToGameObjectComponent.gameObject);
            referenceToGameObjectComponent.gameObject = null;
        }
    }
}