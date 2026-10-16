using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Gemserk.Triggers.Systems
{
    public class AutoCreateTriggerSystemEntity : MonoBehaviour
    {
        public WorldReference worldReference;
        
        private void Awake()
        {
            var world = worldReference.GetReference(gameObject);
            var triggerSystemEntity = world.CreateEntity();
            triggerSystemEntity.Add(new TriggerSystemComponent()
            {
                triggerSystem = GetComponent<ITriggerSystem>()
            });
        }
    }
}