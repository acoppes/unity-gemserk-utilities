using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers
{
    public abstract class WorldTriggerCondition : TriggerCondition
    {
        public WorldReference worldReference;
        
        protected World world;

        private void OnEnable()
        {
            world = worldReference.GetReference(gameObject);
        }

        private void OnDisable()
        {
            world = null;
        }
    }
}