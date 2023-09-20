using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems
{
    public class DamageNumbersSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public GameObject numbersPrefab;

        private GameObjectPool gameObjectPool;

        public void Init(EcsSystems systems)
        {
            if (numbersPrefab != null)
            {
                gameObjectPool = new GameObjectPool(numbersPrefab, "~DamageNumbers");
            }
        }

        public void Run(EcsSystems systems)
        {
            if (gameObjectPool == null)
            {
                return;
            }
            
            var hitPointsComponents = world.GetComponents<HealthComponent>();
            var positionsComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>()
                         .Exc<DisabledComponent>()
                         .Inc<PositionComponent>().End())
            {
                var hitPoints = hitPointsComponents.Get(entity);
                var positionComponent = positionsComponents.Get(entity);
                
                if (hitPoints.processedDamages.Count == 0)
                {
                    continue;
                }

                var total = 0.0f;
                
                foreach (var hit in hitPoints.processedDamages)
                {
                    total += hit.value;
                }

                var damageNumber = gameObjectPool.Get();
                var position = GamePerspective.ConvertFromWorld(positionComponent.value);
                damageNumber.transform.position = position;
                
                var text = damageNumber.GetComponentInChildren<Text>();
                text.text = $"{total:0}";

                LeanTween.moveY(damageNumber, position.y + 2.5f, 0.35f)
                    .setFrom(position.y + 1)
                    .setEaseOutQuad()
                    .setOnCompleteParam(damageNumber)
                    .setOnComplete(delegate(object o)
                    {
                        var gameObject = o as GameObject;
                        gameObjectPool.Release(gameObject);
                    });
            }
        }


    }
}