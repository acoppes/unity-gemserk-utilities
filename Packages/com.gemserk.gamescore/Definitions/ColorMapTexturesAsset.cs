using UnityEngine;

namespace Game.Definitions
{
    [CreateAssetMenu(menuName="Gemserk/Color Map Textures")]
    public class ColorMapTexturesAsset : ScriptableObject
    {
        public Texture2D[] colorTextures;
    }
}