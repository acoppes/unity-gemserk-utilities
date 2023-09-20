using Game.Screens;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct NumberComponent : IEntityComponent
    {
        public string format;
        public float value;
        public bool started;
        public float time;
        public bool disableAnimation;
        
        public GameObject instance;
        public TextView textView;

    }
    
    public class NumberComponentDefinition : ComponentDefinitionBase
    {
        public string defaultFormat = "{0:0}";
        public float defaultValue;
        public float time = 0.35f;
        
        public override string GetComponentName()
        {
            return nameof(NumberComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new NumberComponent()
            {
                format = defaultFormat,
                value = defaultValue,
                time = time
            });
        }
    }
}