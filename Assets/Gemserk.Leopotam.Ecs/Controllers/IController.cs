namespace Gemserk.Leopotam.Ecs.Controllers
{
    public interface IController
    {
        void OnInit(World world, int entity);
        
        void OnUpdate(float dt, World world, int entity);
    }
}