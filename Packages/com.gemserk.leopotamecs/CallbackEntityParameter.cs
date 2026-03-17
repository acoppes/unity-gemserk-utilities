using System;

namespace Gemserk.Leopotam.Ecs
{
    public class CallbackEntityParameter : IEntityInstanceParameter
    {
        private readonly Action<World, Entity> callback;

        public CallbackEntityParameter(Action<World, Entity> callback)
        {
            this.callback = callback;
        }
        
        public void Apply(World world, Entity entity)
        {
            callback(world, entity);
        }
    }
}