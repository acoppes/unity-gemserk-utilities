namespace Gemserk.Leopotam.Ecs.Controllers
{
    public interface IController
    {
        void OnUpdate(float dt, World world, int entity);
    }
}