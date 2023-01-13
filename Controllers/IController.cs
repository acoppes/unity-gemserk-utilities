namespace Gemserk.Leopotam.Ecs.Controllers
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