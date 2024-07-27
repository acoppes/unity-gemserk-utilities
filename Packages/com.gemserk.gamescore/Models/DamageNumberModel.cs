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
        
        private Action<object> callback;

        public void Play(Vector3 position, float total, Action<object> callback)
        {
            this.callback = callback;
            
            transform.position = position + defaultOffset;
            
            text.text = $"{total:0}";

            if (animator != null)
            {
                animator.SetTrigger(Enter);
            }
            else
            {
                LeanTween.moveY(gameObject, position.y, 0.35f)
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