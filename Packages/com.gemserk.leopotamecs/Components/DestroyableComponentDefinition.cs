namespace Gemserk.Leopotam.Ecs.Components
{
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
    
    public class DestroyableComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyableComponent());
        }
    }
}