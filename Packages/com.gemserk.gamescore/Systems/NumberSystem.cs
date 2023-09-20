using Game.Components;
using Game.Screens;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class NumberSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public GameObject numbersPrefab;
        private GameObjectPool gameObjectPool;
        
        readonly EcsFilterInject<Inc<NumberComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;

        public void Init(EcsSystems systems)
        {
            gameObjectPool = new GameObjectPool(numbersPrefab, "~Numbers");
        }
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<NumberComponent>(entity))
            {
                ref var numberComponent = ref world.GetComponent<NumberComponent>(entity);
                numberComponent.instance = gameObjectPool.Get();
                numberComponent.textView = numberComponent.instance.GetComponentInChildren<TextView>();
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<NumberComponent>(entity))
            {
                ref var numberComponent = ref world.GetComponent<NumberComponent>(entity);
                if (numberComponent.instance != null)
                {
                    gameObjectPool.Release(numberComponent.instance);
                }
                
                numberComponent.instance = null;
                numberComponent.textView = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var numberComponent = ref filter.Pools.Inc1.Get(entity);
                var positionComponent = filter.Pools.Inc2.Get(entity);

                if (numberComponent.started)
                {
                    continue;
                }

                var position = positionComponent.value;

                numberComponent.instance.transform.position = position;
                numberComponent.textView.SetText(string.Format(numberComponent.format, numberComponent.value));
                
                if (!numberComponent.disableAnimation)
                {
                    LeanTween.moveY(numberComponent.instance, position.y + 1.5f, numberComponent.time)
                        .setFrom(position.y)
                        .setEaseOutQuad();

                    numberComponent.started = true;
                }
            }
        }


    }
}