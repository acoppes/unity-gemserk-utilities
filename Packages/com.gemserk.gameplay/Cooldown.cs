using System;
using UnityEngine;

namespace Gemserk.Gameplay
{
    [Serializable]
    public struct Cooldown
    {
        private float current;
        
        [SerializeField]
        private float total;

        public Cooldown(float total)
        {
            this.total = total;
            this.current = 0;
        }
        
        public static Cooldown operator +(Cooldown cooldown, float time)
        {
            cooldown.Increase(time);
            return cooldown;
        }

        public void Increase(float time)
        {
            current += time;
            current = Mathf.Clamp(current, 0, total);
        }
        
        public void Decrease(float time)
        {
            current -= time;
            current = Mathf.Clamp(current, 0, total);
        }

        public void Reset()
        {
            current = 0;
        }

        public void Fill()
        {
            current = total;
        }
        
        public void SetTotal(float cooldown)
        {
            this.total = cooldown;
        }

        public bool IsEmpty => current <= 0;

        public bool IsReady => current >= total;

        public float Progress => Mathf.Clamp(current / total, 0f, 1f);

        public float Total => total;
    }
}