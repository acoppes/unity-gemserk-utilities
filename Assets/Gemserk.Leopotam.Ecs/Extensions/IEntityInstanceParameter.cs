namespace Gemserk.Leopotam.Ecs.Extensions
{
    public interface IEntityInstanceParameter
    {
        void Apply(World world, int entity);
    }
}