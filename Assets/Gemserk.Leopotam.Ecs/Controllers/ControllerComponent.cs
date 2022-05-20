namespace Gemserk.Leopotam.Ecs.Controllers
{
    public struct ControllerComponent : IEntityComponent
    {
        public IController controller;
        public bool intialized;
    }
}