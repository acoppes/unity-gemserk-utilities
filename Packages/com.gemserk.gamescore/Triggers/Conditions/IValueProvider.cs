using Gemserk.Leopotam.Ecs;

namespace Game.Triggers.Conditions
{
    public interface IValueProvider
    {
        float GetValue(World world, object activator);
    }
}