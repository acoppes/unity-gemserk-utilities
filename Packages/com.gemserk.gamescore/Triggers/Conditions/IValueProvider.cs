using Gemserk.Leopotam.Ecs;

namespace Game.Triggers.Conditions
{
    public interface IValueProvider
    {
        int GetIntValue(World world, object activator);
    }
}