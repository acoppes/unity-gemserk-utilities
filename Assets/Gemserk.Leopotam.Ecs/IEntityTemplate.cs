namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityDefinition
    {
        void Apply(World world, int entity);
    }
}