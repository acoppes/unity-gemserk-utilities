using System.Collections.Generic;
using UnityEngine;

namespace Game.Utilities
{
    // to be used in bootstrap scene when the game starts to fix texture filters
    public class FixPixelArtFontsTextureFilter : MonoBehaviour
    {
        public List<Font> fonts;

        private void Awake()
        {
            foreach (var font in fonts)
            {
                if (font && font.material)
                {
                    var texture = font.material.mainTexture;
                    if (texture)
                    {
                        texture.filterMode = FilterMode.Point;
                    }
                }
            }
        }
    }
}