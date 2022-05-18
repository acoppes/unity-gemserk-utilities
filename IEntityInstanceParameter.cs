namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityInstanceParameter
    {
        void Apply(World world, int entity);
    }
}