using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers
{
    public abstract class WorldTriggerAction : TriggerAction
    {
        protected World world;

        private void OnEnable()
        {
            world = World.Instance;
        }

        private void OnDisable()
        {
            world = null;
        }
    }
}