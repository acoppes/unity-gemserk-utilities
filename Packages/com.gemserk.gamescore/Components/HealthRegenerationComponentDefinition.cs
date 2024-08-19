using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine.Serialization;

namespace Game.Components
{
    public struct HealthRegenerationComponent : IEntityComponent
    {
        public enum RegenerationType
        {
            PerTick = 0,
            PerTime = 1
        }
        
        public bool enabled;
        public float deltaHealth;

        public RegenerationType regenerationType;
    }
    
    public class HealthRegenerationComponentDefinition : ComponentDefinitionBase
    {
        // public float tick;
        public HealthRegenerationComponent.RegenerationType regenerationType;
        
        public bool startsEnabled;
        [FormerlySerializedAs("regenerationPerTick")] 
        public float deltaHealth;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HealthRegenerationComponent
            {
                regenerationType = regenerationType,
                enabled = startsEnabled,
                deltaHealth = deltaHealth
            });
        }
    }
}