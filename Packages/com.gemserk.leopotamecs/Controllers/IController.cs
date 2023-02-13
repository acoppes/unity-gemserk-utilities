namespace Gemserk.Leopotam.Ecs.Controllers
{
    public interface IUpdate
    {
        void OnUpdate(World world, Entity entity, float dt);
    }
    
    public interface IController
    {
        
    }
}