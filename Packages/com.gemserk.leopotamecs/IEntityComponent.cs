namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityComponent
    {
        
    }
    
    // Just to mark components that meant to be one instance
    public interface ISingletonEntityComponent : IEntityComponent
    {
        
    }

    public interface IEventComponent : IEntityComponent
    {
        
    }

    public interface IActionComponent : IEntityComponent
    {
        
    }

    public interface ITagComponent : IEntityComponent
    {
        
    }
}