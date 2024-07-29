using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Models
{
    public class DamageNumberModel : MonoBehaviour
    {
        private static readonly int Enter = Animator.StringToHash("Enter");
        
        public Text text;

        public Animator animator;

        public Vector3 defaultOffset = new Vector3(0, 2.5f, 0f);
        public Vector3 randomOffset = new Vector3(0, 0, 0f);

        public string numberFormat = "{0:0}";
        
        private Action<object> callback;

        public void Play(Vector3 position, float total, Action<object> callback)
        {
            this.callback = callback;

            var positionOffset = new Vector3(
                UnityEngine.Random.Range(-randomOffset.x, randomOffset.x), 
                UnityEngine.Random.Range(-randomOffset.y, randomOffset.y), 
                0);
            
            transform.position = position + defaultOffset + positionOffset;
            
            // text.text = $"{total:0}";
            text.text = string.Format(numberFormat, total);

            if (animator != null)
            {
                animator.SetTrigger(Enter);
            }
            else
            {
                LeanTween.moveY(gameObject, position.y + positionOffset.y, 0.35f)
                    .setFrom(position.y + 1)
                    .setEaseOutQuad()
                    .setOnCompleteParam(gameObject)
                    .setOnComplete(callback);
            }
        }

        public void OnAnimationComplete()
        {
            callback?.Invoke(gameObject);
        }
    }
}