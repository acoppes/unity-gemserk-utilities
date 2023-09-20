using UnityEngine;

namespace Game
{
    public static class TimeExtensions
    {
        public static int FixedFrameCount => Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
    }
}