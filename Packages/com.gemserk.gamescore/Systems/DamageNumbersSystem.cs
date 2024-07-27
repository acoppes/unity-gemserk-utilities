using System;
using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems
{
    public class DamageNumbersSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent, PositionComponent>, Exc<DisabledComponent>> 
            filter = default;
        
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
            
            foreach (var e in filter.Value)
            {
                ref var health = ref filter.Pools.Inc1.Get(e);
                ref var positionComponent = ref filter.Pools.Inc2.Get(e);

                if (health.processedDamages.Count == 0)
                {
                    continue;
                }

                var total = 0.0f;

                for (var i = 0; i < health.processedDamages.Count; i++)
                {
                    var hit = health.processedDamages[i];
                    total += hit.value;
                }

                var damageNumberGameObject = gameObjectPool.Get();
                var model = damageNumberGameObject.GetComponent<DamageNumberModel>();
                
                var position = GamePerspective.ConvertFromWorld(positionComponent.value);
                model.Play(position, total, OnNumberAnimComplete);
                
                // damageNumberGameObject.transform.position = position;
                // var text = damageNumberGameObject.GetComponentInChildren<Text>();
                // text.text = $"{total:0}";
                //
                // LeanTween.moveY(damageNumberGameObject, position.y + 2.5f, 0.35f)
                //     .setFrom(position.y + 1)
                //     .setEaseOutQuad()
                //     .setOnCompleteParam(damageNumberGameObject)
                //     .setOnComplete(OnNumberAnimComplete);
            }
        }

        private void OnNumberAnimComplete(object o)
        {
            var gameObject = o as GameObject;
            gameObjectPool.Release(gameObject);
        }
    }
}