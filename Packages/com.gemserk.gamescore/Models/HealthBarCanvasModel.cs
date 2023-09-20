using UnityEngine;
using UnityEngine.UI;

namespace Game.Models
{
    public class HealthBarCanvasModel : HealthBarModel
    {
        public CanvasGroup canvasGroup;
        public Image backgroundImage;
        public Image borderImage;
        public Image fillImage;
        
        private void LateUpdate()
        {
            canvasGroup.alpha = visible ? 1 : 0;
            fillImage.fillAmount = fillAmount;

            if (isMainPlayer)
            {
                backgroundImage.color = backgroundColors[0];
                fillImage.color = fillColors[0];
                borderImage.color = borderColors[0];
            }
            else
            {
                backgroundImage.color = backgroundColors[1];
                fillImage.color = fillColors[1];
                borderImage.color = borderColors[1];
            }
        }
    }
}