using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class ReferenceToGameObjectSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
{
    readonly EcsPoolInject<ReferenceToGameObjectComponent> components = default;
    
    public void OnEntityCreated(World world, Entity entity)
    {
        if (components.Value.Has(entity))
        {
            ref var referenceToGameObjectComponent = ref components.Value.Get(entity);
            referenceToGameObjectComponent.gameObject = new GameObject($"GAMEOBJECT_{entity}");
     
            var singletons = world.GetComponents<NameComponent>();
            if (singletons.Has(entity))
            {
                referenceToGameObjectComponent.gameObject.name = $"GAMEOBJECT_{singletons.Get(entity).name}";
            }
        }
    }

    public void OnEntityDestroyed(World world, Entity entity)
    {
        if (components.Value.Has(entity))
        {
            ref var referenceToGameObjectComponent = ref components.Value.Get(entity);
            Destroy(referenceToGameObjectComponent.gameObject);
            referenceToGameObjectComponent.gameObject = null;
        }
    }
}