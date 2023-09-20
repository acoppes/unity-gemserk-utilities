using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ModelRecolorSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent, ModelRemapTexturePerPlayerComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var modelComponent = filter.Pools.Inc1.Get(e);
                var remapTexturePerPlayerComponent = filter.Pools.Inc2.Get(e);

                if (modelComponent.instance.remapShader != null && remapTexturePerPlayerComponent.remapTexturesPerPlayer.Length > 0)
                {
                    modelComponent.instance.remapShader.lutTexture = 
                        remapTexturePerPlayerComponent.remapTexturesPerPlayer.GetItemOrLast(remapTexturePerPlayerComponent.textureVariant);
                }
            }
        }
    }
}