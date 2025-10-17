namespace Gemserk.Leopotam.Ecs.Events
{
    public interface IControllerEvent
    {
        
    }
    
    public interface IInit : IControllerEvent
    {
        void OnInit(World world, Entity entity);
    }
    
    public interface IDestroyed : IControllerEvent
    {
        void OnDestroyed(World world, Entity entity);
    }
    
    public interface IConfigurable : IControllerEvent
    {
        void OnConfigured(World world, Entity entity);
    }

    public interface IStateChanged : IControllerEvent
    {
        void OnEnterState(World world, Entity entity);
        
        void OnExitState(World world, Entity entity);
    }
    
    public interface IUpdate : IControllerEvent
    {
        void OnUpdate(World world, Entity entity, float dt);
    }
}