using Game.Models;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Components
{
    public struct ModelParticleSystemComponent : IEntityComponent
    {
        public GameObject particleSystemPrefab;
        public GameObject instance;
        public Model currentModel;
    }
    
    public class ModelParticleSystemComponentDefinition : ComponentDefinitionBase
    {
        public GameObject particleSystemPrefab;

        public override void Apply(World world, Entity entity)
        {
            Assert.IsNotNull(particleSystemPrefab, $"{gameObject.name}: ModelComponent needs prefab to work");
            
            world.AddComponent(entity, new ModelParticleSystemComponent
            {
                particleSystemPrefab = particleSystemPrefab
            });
        }
    }
}