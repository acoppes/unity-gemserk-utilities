using Gemserk.Leopotam.Ecs;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public interface IUpdate
    {
        void OnUpdate(float dt);
    }
    
    public interface IController
    {
        void Bind(World world, Entity entity);
    }
}