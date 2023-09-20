using UnityEngine;

namespace Game.Models
{
    [ExecuteInEditMode]
    public class SpriteColorRemapShader : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        public Texture2D lutTexture;

        public Material spriteWithLutMaterial;

        private void ReconfigurePropertyBlock()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (lutTexture == null)
            {
                return;
            }

            if (spriteWithLutMaterial == null)
            {
                return;
            }

            spriteRenderer.sharedMaterial = spriteWithLutMaterial;
            
            var propertyBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propertyBlock);
            
            propertyBlock.SetTexture("_LutTex", lutTexture);
            
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }
    
        // private void OnEnable()
        // {
        //     ReconfigurePropertyBlock();
        // }

        private void LateUpdate()
        {
            ReconfigurePropertyBlock();
        }
    }
}
