using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers
{
    public abstract class WorldTriggerAction : TriggerAction
    {
        protected World world;

        private void OnEnable()
        {
            world = World.Default;
        }

        private void OnDisable()
        {
            world = null;
        }
    }
}