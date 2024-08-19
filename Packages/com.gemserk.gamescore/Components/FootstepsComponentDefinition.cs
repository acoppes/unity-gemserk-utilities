using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Components
{
    public struct FootstepsComponent : IEntityComponent
    {
        public IEntityDefinition sfxDefinition;
        
        public GameObject particlesPrefab;
        
        public IEntityDefinition decalDefinition;
        public float spawnDistance;
        public float randomOffset;
        
        public ParticleSystem walkingParticleSystem;
        public Vector2 lastSpawnPosition;

        public bool isWalking;

        public bool disabled;
    }
    
    public class FootstepsComponentDefinition : ComponentDefinitionBase
    {
        public UnityEngine.Object decalDefinition;
        public float spawnDistance = 0.75f;
        public float randomOffset = 0.2f;
        
        public UnityEngine.Object sfxDefinition;
        public GameObject particlesPrefab;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new FootstepsComponent()
            {
                sfxDefinition = sfxDefinition.GetInterface<IEntityDefinition>(),
                particlesPrefab = particlesPrefab,
                decalDefinition = decalDefinition.GetInterface<IEntityDefinition>(),
                spawnDistance = spawnDistance,
                randomOffset = randomOffset
            });
        }
    }
}