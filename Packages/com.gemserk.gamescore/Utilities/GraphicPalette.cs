using Game.DataAssets;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utilities
{
    [ExecuteInEditMode]
    public class GraphicPalette : MonoBehaviour
    {
        private static readonly int PaletteTexturePropertyID = Shader.PropertyToID("_PaletteTex");

        public Graphic image;
        
        public Shader shader;

        public ColorSet colorSet;
        
        private Texture2D palette;
        private Material material;

        public bool updateInRuntime;

        private void CreateMaterial()
        {
            if (!shader)
                return;

            if (material)
                return;

            if (!colorSet)
                return;

            if (palette != null)
            {
                GameObject.DestroyImmediate(palette);
                palette = null;
            }
            
            palette = colorSet.CreateLutTexture();
            
            material = new Material(shader);
            material.SetTexture(PaletteTexturePropertyID, palette);
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
            if (material)
            {
                if (updateInRuntime)
                {
                    if (palette != null)
                    {
                        GameObject.DestroyImmediate(palette);
                        palette = null;
                    }
                    palette = colorSet.CreateLutTexture();
                    material.SetTexture(PaletteTexturePropertyID, palette);
                }
                
                image.material = material;
            }
            
            // CreateMaterial();
            //
            // if (shader != null && material != null)
            // {
            //     material.SetTexture(PaletteTexturePropertyID, palette);
            //     Graphics.Blit(src, dst, material);
            // }
        }
    }
}