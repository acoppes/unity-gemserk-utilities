namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityInstanceParameter
    {
        void Apply(World world, Entity entity);
    }
}