namespace Gemserk.Leopotam.Ecs.Components
{
    // instantly destroy this
    public struct DestroyableComponent : IEntityComponent
    {
        // private bool _destroy;
        public bool destroy;
        // public bool destroy
        // {
        //     get => _destroy;
        //     set
        //     {
        //         if (value)
        //         {
        //             Debug.Log(new System.Diagnostics.StackTrace());
        //         }
        //         _destroy = value;
        //     }
        // }
    }

    // disables object and destroy in specified frames
    public struct DelayedDestroyComponent : IEntityComponent
    {
        public int frames;
    }
    
    public class DestroyableComponentDefinition : ComponentDefinitionBase
    {
        public int framesToDestroy = 0;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyableComponent());
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