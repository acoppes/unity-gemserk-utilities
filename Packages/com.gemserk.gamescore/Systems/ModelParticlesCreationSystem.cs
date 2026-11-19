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
                
                // keep prefab declared model to restore later for reuse.
                modelParticle.previousModel = instance.model;
                
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
                    if (modelParticle.currentModel.particleSystem)
                    {
                        modelParticle.currentModel.particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                    
                    modelParticle.currentModel.particleSystem = null;
                    
                    // restore previous model from prefab :S
                    modelParticle.currentModel.model = modelParticle.previousModel;
                }
                
                if (modelParticle.instance)
                {
                    modelParticle.instance.transform.SetParent(null, false);
                    modelParticle.instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    modelParticle.instance.transform.localScale = Vector3.one;
                    
                    particlesPoolMap.Release(modelParticle.particleSystemPrefab, modelParticle.instance);
                }
                
                modelParticle.currentModel = null;
                modelParticle.instance = null;
            }
        }


    }
}