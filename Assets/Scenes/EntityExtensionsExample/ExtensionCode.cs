using Gemserk.Leopotam.Ecs;

namespace Scenes.EntityExtensionsExample
{
    public static class ExtensionCode {

        public static TargetComponent GetTargetComponent(this Entity entity)
        {
            return entity.GetComponent<TargetComponent>();
        }
    }

    public class TestCode
    {
        public void Code()
        {
            var entity = new Entity();

            var target = entity.GetTargetComponent();
        }
    }
}
