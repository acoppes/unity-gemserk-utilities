using Gemserk.Leopotam.Ecs;

namespace Scenes.EntityExtensionsExample
{
    public static class ExtensionCode {

        public static TargetComponent GetTargetComponent(this Entity entity)
        {
            return entity.Get<TargetComponent>();
        }
    }
}
