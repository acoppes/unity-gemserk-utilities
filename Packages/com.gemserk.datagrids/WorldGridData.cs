using System.Runtime.CompilerServices;
using UnityEngine;

namespace Gemserk.DataGrids
{
    public struct WorldGridData
    {
        public readonly Vector2 worldSize;
        public readonly Vector2 gridSize;

        public readonly int width;
        public readonly int height;

        public WorldGridData(Vector2 worldSize, Vector2 gridSize)
        {
            this.gridSize = gridSize;
            this.worldSize = worldSize;

            width = Mathf.CeilToInt(worldSize.x / gridSize.x);
            height = Mathf.CeilToInt(worldSize.y / gridSize.y);
        }

        public Grid<T> CreateGrid<T>(T defaultValue) where T : struct
        {
            var grid = new Grid<T>(Mathf.CeilToInt(worldSize.x / gridSize.x), 
                Mathf.CeilToInt(worldSize.y / gridSize.y), defaultValue);
            return grid;
        }

        public Vector2Int GetGridPosition(Vector2 position)
        {
            var x = Mathf.RoundToInt((position.x + worldSize.x * 0.5f) / gridSize.x);
            var y = Mathf.RoundToInt((position.y + worldSize.y * 0.5f) / gridSize.y);
            return new Vector2Int(x, y);
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(
                x * gridSize.x - worldSize.x * 0.5f,
                y * gridSize.y - worldSize.y * 0.5f);
        }
        
        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return new Vector2(
                gridPosition.x * gridSize.x - worldSize.x * 0.5f,
                gridPosition.y * gridSize.y - worldSize.y * 0.5f);
        }
        
        public Vector2 AdjustWorldPosition(Vector2 worldPosition)
        {
            return GetWorldPosition(GetGridPosition(worldPosition));
        }
        
        public void StoreValue<T>(Grid<T> grid, T value, Vector2 worldPosition) where T: struct
        {
            var gridPosition = GetGridPosition(worldPosition);
            if (!grid.IsInside(gridPosition.x, gridPosition.y)) 
                return;
            grid.StoreValue(value, gridPosition.x, gridPosition.y);
        }
        
        public void StoreValue<T>(Grid<T> grid, T value, Vector2Int gridPosition) where T: struct
        {
            if (!grid.IsInside(gridPosition.x, gridPosition.y)) 
                return;
            grid.StoreValue(value, gridPosition.x, gridPosition.y);
        }

        public T GetValue<T>(Grid<T> grid, Vector2 worldPosition, T defaultValue) where T : struct
        {
            var gridPosition = GetGridPosition(worldPosition);
            return grid.IsInside(gridPosition.x, gridPosition.y) ? 
                grid.ReadValue(gridPosition.x, gridPosition.y) : defaultValue;
        }
        
        public T GetValue<T>(Grid<T> grid, Vector2Int gridPosition, T defaultValue) where T : struct
        {
            return grid.IsInside(gridPosition.x, gridPosition.y) ? 
                grid.ReadValue(gridPosition.x, gridPosition.y) : defaultValue;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(int i, int j)
        {
            return i >= 0 && i < width && j >= 0 && j < height;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(Vector2 worldPosition)
        {
            var gridPosition = GetGridPosition(worldPosition);
            return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreFlagValue(Grid<int> gridData, int value, Vector2 position)
        {
            var p = GetGridPosition(position);
            if (!gridData.IsInside(p.x, p.y)) 
                return;

            var v = gridData.ReadValue(p.x, p.y);
            gridData.StoreValue(v | value, p.x, p.y);
            
            // has flag value
            //    return (values[i + j * width] & value) > 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlag(Grid<int> gridData, int value, Vector2 position)
        {
            var p = GetGridPosition(position);
            if (!gridData.IsInside(p.x, p.y)) 
                return false;
            
            var v = gridData.ReadValue(p.x, p.y);
            return (v & value) > 0;
        }
    }
}
