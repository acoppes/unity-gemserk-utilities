using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class DamageNumbersSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<HealthComponent, PositionComponent, HealthDamageNumberComponent>, Exc<DisabledComponent>> 
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
                ref var damageNumber = ref filter.Pools.Inc3.Get(e);
                
                if (health.processedDamages.Count == 0)
                {
                    continue;
                }

                for (var i = 0; i < health.processedDamages.Count; i++)
                {
                    var hit = health.processedDamages[i];
                    damageNumber.current += hit.value;
                }

                if (damageNumber.current >= damageNumber.minToShow)
                {
                    var damageNumberGameObject = gameObjectPool.Get();
                    var model = damageNumberGameObject.GetComponent<DamageNumberModel>();

                    var position = positionComponent.value;
                
                    if (positionComponent.type == 0)
                    {
                        position = GamePerspective.ConvertFromWorld(positionComponent.value);
                    }
                
                    model.Play(position, damageNumber.current, OnNumberAnimComplete);

                    damageNumber.current = 0;
                }


            }
        }

        private void OnNumberAnimComplete(object o)
        {
            var gameObject = o as GameObject;
            gameObjectPool.Release(gameObject);
        }
    }
}