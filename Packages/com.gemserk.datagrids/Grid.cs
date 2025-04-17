using System;
using System.Runtime.CompilerServices;

namespace Gemserk.DataGrids
{
    public struct Grid<T> where T: struct
    {
        public int width;
        public int height;

        public readonly T[] values;

        public int Length => values.Length;
        
        public Grid(int width, int height, T defaultValue)
        {
            this.width = width;
            this.height = height;

            var length = width * height;
			
            values = new T[length];
            Clear(defaultValue);
        }
        
        public Grid(int width, int height, T[] initialValues)
        {
            this.width = width;
            this.height = height;

            var length = width * height;
			
            values = new T[length];
            Array.Copy(initialValues, values, values.Length);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(int i, int j)
        {
            return i >= 0 && i < width && j >= 0 && j < height;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreValue(T value, int i, int j)
        {
            values[i + j * width] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ReadValue(int i, int j)
        {
            return values[i + j * width];
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ReadValue(int i)
        {
            return values[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]    
        public bool IsValue(T value, int i, int j)
        {
            return values[i + j * width].Equals(value);
        }
		
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValue(T value, int i)
        {
            return values[i].Equals(value);
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
        public void Clear(T value)
        {
            Array.Fill(values, value, 0, values.Length);
        }
    }
}