using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class EffectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EffectsComponent, PositionComponent>, Exc<DisabledComponent>> effectsFilter = default;
        readonly EcsFilterInject<Inc<EffectsComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyableEffectsFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in effectsFilter.Value)
            {
                // var cursor = ref cursorInputFilter.Pools.Inc1.Get(e);
                ref var effects = ref effectsFilter.Pools.Inc1.Get(e);
                var position = effectsFilter.Pools.Inc2.Get(e);
                
                foreach (var effect in effects.effects)
                {
                    if (effects.target != null && effects.target.entity.Exists())
                    {
                        ref var health = ref effects.target.entity.Get<HealthComponent>();
                        health.damages.Add(new DamageData()
                        {
                            value = effect.value,
                            position = position.value,
                            knockback = false,
                            source = effects.source
                        });
                    }
                }
            }
            
            foreach (var e in destroyableEffectsFilter.Value)
            {
                ref var destroyable = ref destroyableEffectsFilter.Pools.Inc2.Get(e);
                destroyable.destroy = true;
            }
        }
    }
}