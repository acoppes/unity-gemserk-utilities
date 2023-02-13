namespace Gemserk.Leopotam.Ecs.Events
{
    public interface IInit
    {
        void OnInit(World world, Entity entity);
    }

    public interface IEntityDestroyed
    {
        void OnEntityDestroyed(World world, Entity entity);
    }

    public interface IConfigurable
    {
        void OnConfigured(World world, Entity entity);
    }

    public interface IStateChanged
    {
        void OnEnterState(World world, Entity entity);
        
        void OnExitState(World world, Entity entity);
    }
}