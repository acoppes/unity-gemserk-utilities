using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class ModelParticlesCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler,
        IEcsInitSystem
    {
        private GameObjectPoolMap particlesPoolMap;
        
        public void Init(EcsSystems systems)
        {
            particlesPoolMap = new GameObjectPoolMap("~ParticleModelsPool");
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<ModelInstanceComponent>() && entity.Has<ModelParticleSystemComponent>())
            {
                ref var modelParticle = ref entity.Get<ModelParticleSystemComponent>();
                modelParticle.instance = particlesPoolMap.Get(modelParticle.particleSystemPrefab);
                
                var instance = entity.Get<ModelInstanceComponent>().instance;
                instance.particleSystem = modelParticle.instance.GetComponent<ParticleSystem>();
                instance.model = modelParticle.instance.transform;
            
                modelParticle.instance.transform.SetParent(instance.transform, false);
                modelParticle.currentModel = instance;
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (entity.Has<ModelParticleSystemComponent>())
            {
                ref var modelParticle = ref entity.Get<ModelParticleSystemComponent>();

                if (modelParticle.currentModel)
                {
                    modelParticle.currentModel.particleSystem = null;
                    modelParticle.currentModel.model = null;
                }
                
                if (modelParticle.instance)
                {
                    modelParticle.instance.transform.SetParent(null, false);
                    particlesPoolMap.Release(modelParticle.particleSystemPrefab, modelParticle.instance);
                }
                
                modelParticle.currentModel = null;
                modelParticle.instance = null;
            }
        }


    }
}