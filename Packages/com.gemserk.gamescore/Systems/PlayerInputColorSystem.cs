using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class PlayerInputColorSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ModelRemapTexturePerPlayerComponent, PlayerInputComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var remapTexturePerPlayerComponent = ref filter.Pools.Inc1.Get(e);
                var playerInputComponent = filter.Pools.Inc2.Get(e);
                remapTexturePerPlayerComponent.textureVariant = playerInputComponent.playerInput;
            }
        }
    }
}