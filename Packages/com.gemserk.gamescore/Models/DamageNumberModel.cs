using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Models
{
    public class DamageNumberModel : MonoBehaviour
    {
        public Text text;

        public void Play(Vector3 position, float total, Action<object> callback)
        {
            transform.position = position;
            
            text.text = $"{total:0}";

            LeanTween.moveY(gameObject, position.y + 2.5f, 0.35f)
                .setFrom(position.y + 1)
                .setEaseOutQuad()
                .setOnCompleteParam(gameObject)
                .setOnComplete(callback);
        }
    }
}