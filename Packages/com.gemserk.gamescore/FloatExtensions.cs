using System.Runtime.CompilerServices;

namespace Game
{
    public static class FloatExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Square(this float f)
        {
            return f * f;
        }
    }
}