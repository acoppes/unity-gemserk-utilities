using System.Collections.Generic;
using Game.DataAssets;
using UnityEngine;

namespace Game.Utilities
{
    public class CurrentPaletteAutoSet : MonoBehaviour
    {
        public static int currentPalette = 0;

        public int defaultPalette = 0;
        
        public GraphicPalette graphicPalette;
        public List<ColorSet> palettes;

        private void Awake()
        {
            currentPalette = defaultPalette;
        }

        public void Update()
        {
            graphicPalette.colorSet = palettes[currentPalette];
        }
    }
}