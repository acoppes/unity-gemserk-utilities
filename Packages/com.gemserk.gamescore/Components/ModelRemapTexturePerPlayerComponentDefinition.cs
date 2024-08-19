using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct ModelRemapTexturePerPlayerComponent : IEntityComponent
    {
        public Texture2D[] remapTexturesPerPlayer;
        public int textureVariant;
    }
    
    public class ModelRemapTexturePerPlayerComponentDefinition : ComponentDefinitionBase
    {
        public ColorMapTexturesAsset colorMapTexturesAsset;
        public Texture2D[] remapTexturesPerPlayer;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ModelRemapTexturePerPlayerComponent
            {
                remapTexturesPerPlayer = colorMapTexturesAsset != null ? colorMapTexturesAsset.colorTextures : remapTexturesPerPlayer,
                textureVariant = 0
            });
        }
    }
}