using Game.Systems;
using UnityEngine;

namespace Game.Models
{
    public class HealthBarSpriteModel : HealthBarModel
    {
        public GameObject parent;
        public SpriteRenderer border, background, fill, highlight;
        
        private void LateUpdate()
        {
            parent.SetActive(visible);

            var p = GamePerspective.ConvertFromWorld(position);

            transform.position = new Vector3(p.x, p.y, 0);
            parent.transform.localPosition = new Vector3(0, p.z, 0);

            fill.transform.localScale = new Vector3(fillAmount, 1, 1);

            if (isMainPlayer)
            {
                background.color = backgroundColors[0];
                fill.color = fillColors[0];
                border.color = borderColors[0];
            }
            else
            {
                background.color = backgroundColors[1];
                fill.color = fillColors[1];
                border.color = borderColors[1];
            }

            highlight.enabled = highlighted;
        }
    }
}