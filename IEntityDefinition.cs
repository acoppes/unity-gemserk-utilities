namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityDefinition
    {
        void Apply(World world, Entity entity);
    }
    
    public interface IEntityComponentDefinition
    {
        void Apply(World world, Entity entity);
    }
}