using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class ModelShakeSystem : BaseSystem, IEcsRunSystem
    {
        public float maxIntensityMultiplier = 0.5f;

        public float frameUpdateTime = 2.0f / 15.0f;

        public AnimationCurve intensityCurve = 
            AnimationCurve.Linear(0, 1, 0, 0);
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            var modelShakeComponents = world.GetComponents<ModelShakeComponent>();

            var dt = Time.deltaTime;

            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Inc<ModelShakeComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                ref var modelShakeComponent = ref modelShakeComponents.Get(entity);

                modelShakeComponent.time += dt;
                modelShakeComponent.updateTime += dt;

                if (modelShakeComponent.restart)
                {
                    modelShakeComponent.updateTime = frameUpdateTime;
                    modelShakeComponent.currentOffset = new Vector3(UnityEngine.Random.Range(-1, 1), 0 , 0);
                    modelShakeComponent.restart = false;
                }

                if (modelShakeComponent.time < modelShakeComponent.duration)
                {
                    if (modelShakeComponent.updateTime >= frameUpdateTime)
                    {
                        var intensity =
                            intensityCurve.Evaluate(modelShakeComponent.time / modelShakeComponent.duration) * 
                            modelShakeComponent.intensity;

                        // flip
                        var direction = modelShakeComponent.currentOffset.x < 0 ? 1.0f : -1.0f;
                        var randomPosition = Vector2.right * maxIntensityMultiplier * intensity * direction;

                        modelShakeComponent.currentOffset = new Vector3(randomPosition.x, 0, 0);

                        modelShakeComponent.updateTime = 0;
                    }
                }
                else
                {
                    modelShakeComponent.currentOffset = Vector3.zero;
                }
                
                if (modelComponent.instance != null)
                {
                    modelComponent.instance.spriteRenderer.transform.localPosition +=
                        modelShakeComponent.currentOffset;
                }
            }
        }
    }
}