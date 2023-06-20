using UnityEngine;

namespace Gemserk.Utilities
{
    public static class TimeExtensions
    {
        public static int FixedFrameCount => Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
    }
}