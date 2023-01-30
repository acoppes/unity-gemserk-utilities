using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Beatemup.Triggers
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