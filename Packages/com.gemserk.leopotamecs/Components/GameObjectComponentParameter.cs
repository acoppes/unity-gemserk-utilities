using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Components
{
    public class GameObjectComponentParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public GameObject linkedObject;

        public void Apply(World world, Entity entity)
        {
            if (!entity.Has<GameObjectComponent>())
            {
                entity.Add(new GameObjectComponent());
            }

            ref var goComponent = ref entity.Get<GameObjectComponent>();
            goComponent.gameObject = linkedObject;
        }
    }
}