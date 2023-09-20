using Gemserk.Leopotam.Ecs;

namespace Game.Controllers
{
    public interface IActiveController
    {
        // this could return different levels of interruption, like
        // Important, Reaction, Passive, Any
        bool CanBeInterrupted(Entity entity, IActiveController activeController);

        void OnInterrupt(Entity entity, IActiveController activeController);
    }
}