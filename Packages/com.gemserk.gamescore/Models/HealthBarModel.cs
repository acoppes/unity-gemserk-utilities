using System;
using UnityEngine;

namespace Game.Models
{
    public class HealthBarModel : MonoBehaviour
    {
        public Color[] backgroundColors;
        public Color[] borderColors;
        public Color[] fillColors;
        
        [NonSerialized]
        public bool visible;

        [NonSerialized]
        public bool highlighted;

        [NonSerialized]
        public float fillAmount;
        
        [NonSerialized]
        public bool isMainPlayer;

        [NonSerialized]
        public Vector3 position;
    }
}