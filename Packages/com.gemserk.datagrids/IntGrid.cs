using System;
using System.Runtime.CompilerServices;

namespace Gemserk.DataGrids
{
    public struct IntGrid
    {
        public int width;
        public int height;

        public readonly int[] values;

        public int Length => values.Length;
        
        public IntGrid(int width, int height, int defaultValue = 0)
        {
            this.width = width;
            this.height = height;

            var length = width * height;
			
            values = new int[length];
            Clear(defaultValue);
        }
        
        public IntGrid(int width, int height, int[] initialValues)
        {
            this.width = width;
            this.height = height;

            var length = width * height;
			
            values = new int[length];
            Array.Copy(initialValues, values, values.Length);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(int i, int j)
        {
            return i >= 0 && i < width && j >= 0 && j < height;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreValue(int value, int i, int j)
        {
            values[i + j * width] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadValue(int i, int j)
        {
            return values[i + j * width];
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadValue(int i)
        {
            return values[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]    
        public bool IsValue(int value, int i, int j)
        {
            return values[i + j * width] == value;
        }
		
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValue(int value, int i)
        {
            return values[i] == value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearValues()
        {
            Array.Clear(values, 0, values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(values, 0, values.Length);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(int value)
        {
            Array.Fill(values, value, 0, values.Length);
        }
    }
}