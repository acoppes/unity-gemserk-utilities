using Gemserk.Leopotam.Ecs;
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
        public float regeneration;

        public RegenerationType regenerationType;
        
        public float regenerationDelayTotal;
        public float regenerationDelayCurrent;

        public bool wasActive;
        public bool isActive;
    }
    
    public class HealthRegenerationComponentDefinition : ComponentDefinitionBase
    {
        // public float tick;
        public HealthRegenerationComponent.RegenerationType regenerationType;
        
        public bool startsEnabled;
        [FormerlySerializedAs("deltaHealth")]
        [FormerlySerializedAs("regenerationPerTick")] 
        public float regeneration;
        
        [FormerlySerializedAs("damageRegenerationDisableTime")] 
        public float regenerationDelayTotal;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HealthRegenerationComponent
            {
                regenerationType = regenerationType,
                enabled = startsEnabled,
                regeneration = regeneration,
                regenerationDelayTotal = regenerationDelayTotal
            });
        }
    }
}