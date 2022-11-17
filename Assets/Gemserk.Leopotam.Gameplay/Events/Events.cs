using Gemserk.Leopotam.Ecs;

namespace Gemserk.Leopotam.Gameplay.Events
{
    public interface IInit
    {
        void OnInit();
    }
    
    public interface IEntityDestroyed
    {
        void OnEntityDestroyed(Entity e);
    }

    public interface IConfigurable
    {
        void OnConfigured();
    }

    public interface IStateChanged
    {
        void OnEnter();
        
        void OnExit();
    }
}