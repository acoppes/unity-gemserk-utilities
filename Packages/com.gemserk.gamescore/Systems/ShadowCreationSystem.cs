using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;

namespace Game.Systems
{
    public class ShadowCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(World world, Entity entity)
        {
            var hasShadowComponents = world.GetComponents<HasShadowComponent>();
            if (hasShadowComponents.Has(entity))
            {
                ref var hasShadowComponent = ref hasShadowComponents.Get(entity);

                if (hasShadowComponent.shadowDefintion != null)
                {
                    hasShadowComponent.shadowEntity = world.CreateEntity(hasShadowComponent.shadowDefintion);
                    ref var shadowComponent = ref world.GetComponent<ShadowComponent>(hasShadowComponent.shadowEntity);
                    shadowComponent.target = entity;
                } 
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            var hasShadowComponents = world.GetComponents<HasShadowComponent>();
            if (hasShadowComponents.Has(entity))
            {
                ref var hasShadowComponent = ref hasShadowComponents.Get(entity);
                if (world.Exists(hasShadowComponent.shadowEntity))
                {
                    ref var destroyableComponent = ref world.GetComponent<DestroyableComponent>(hasShadowComponent.shadowEntity);
                    destroyableComponent.destroy = true;
                }
            }
        }
    }
}