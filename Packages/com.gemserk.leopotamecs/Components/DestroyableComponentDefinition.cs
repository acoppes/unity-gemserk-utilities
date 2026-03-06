namespace Gemserk.Leopotam.Ecs.Components
{
    public struct DestroyableComponent : IEntityComponent
    {
        public bool destroy;
        public bool signalOnDestroy;
    }

    // disables object and destroy in specified frames
    public struct DelayedDestroyComponent : IEntityComponent
    {
        public int frames;
    }
    
    public class DestroyableComponentDefinition : ComponentDefinitionBase
    {
        public int framesToDestroy = 0;
        public bool signalOnDestroy;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyableComponent()
            {
                signalOnDestroy = signalOnDestroy
            });
            
            if (framesToDestroy > 0)
            {
                world.AddComponent(entity, new DelayedDestroyComponent()
                {
                    frames = framesToDestroy
                });
            }
        }
    }
}