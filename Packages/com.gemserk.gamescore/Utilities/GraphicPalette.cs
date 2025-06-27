using Game.DataAssets;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utilities
{
    // [ExecuteInEditMode]
    public class GraphicPalette : MonoBehaviour
    {
        private static readonly int PaletteTexturePropertyID = Shader.PropertyToID("_PaletteTex");

        public Graphic image;
        
        public Shader shader;

        public ColorSet colorSet;
        
        private Texture2D palette;
        private Material material;
        
        #if UNITY_EDITOR
        public bool forceRefresh;
        #else 
        private bool forceRefresh = false;
        #endif
        
        public void SetColorSet(ColorSet newColorSet)
        {
            if (!colorSet || newColorSet != colorSet)
            {
                colorSet = newColorSet;
                RegeneratePalette();
            }
        }

        private void CreateMaterial()
        {
            if (!shader)
                return;
            
            if (!colorSet)
                return;
            
            if (material)
                return;
            
            RegeneratePalette();
        }

        private void RegeneratePalette()
        {
            if (palette)
            {
                DestroyImmediate(palette);
                palette = null;
            }
            
            palette = colorSet.CreateLutTexture();
            
            material = new Material(shader);
            material.SetTexture(PaletteTexturePropertyID, palette);
            
            image.material = material;
        }

        private void DestroyMaterial()
        {
            if (material)
            {
                DestroyImmediate(material);
                material = null;
            }
        }

        private void OnEnable()
        {
            CreateMaterial();
        }

        private void OnDisable()
        {
            DestroyMaterial();
        }

        private void Update()
        {
            if (forceRefresh)
            {
                RegeneratePalette();
            }
        }
    }
}