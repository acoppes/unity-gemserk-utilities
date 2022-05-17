namespace Gemserk.Leopotam.Ecs.Extensions
{
    public interface IEntityDefinition
    {
        void Apply(World world, int entity);
    }
}