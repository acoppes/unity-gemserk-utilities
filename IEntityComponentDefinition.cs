namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityComponentDefinition
    {
        void Apply(World world, Entity entity);
    }
}