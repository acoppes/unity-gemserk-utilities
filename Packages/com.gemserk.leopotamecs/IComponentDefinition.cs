namespace Gemserk.Leopotam.Ecs
{
    public interface IComponentDefinition
    {
        void Apply(World world, Entity entity);
    }
}