using System;

namespace Gemserk.Vision
{
    public struct CachedIntAbsoluteValues
    {
        public int width;

        public int[] cache;

        public CachedIntAbsoluteValues(int width)
        {
            this.width = width;
            cache = new int[width * 2];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Store(i - j, Math.Abs(i - j));
                }
            }
        }

        private void Store(int x, int value)
        {
            cache[x + width] = value;
        }

        public int Abs(int x)
        {
            return cache[x + width];
        } 
    }
}