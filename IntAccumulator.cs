using UnityEngine;

namespace Gemserk.Utilities
{
    public struct IntAccumulator
    {
        public int current;
        public int total;

        public void Increase(int i)
        {
            current += i;
            current = Mathf.Clamp(current, 0, total);
        }
        
        public void Decrease(int i)
        {
            current -= i;
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

        public bool IsEmpty() => current == 0;

        public bool IsFull() => current == total;

        public float Progress() => Mathf.Clamp(current / (float) total, 0f, 1f);
    }
}