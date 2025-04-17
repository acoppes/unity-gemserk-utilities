using System;
using System.Collections.Generic;
using Gemserk.DataGrids;
using UnityEngine;

namespace Gemserk.Vision
{
    public struct VisionMatrix  {

        public CachedIntAbsoluteValues cachedAbsoluteValues;
        
        public int width;
        public int height;

        public bool raycastEnabled;

        public Grid<int> visionData;
        public Grid<int> temporaryVisibleData;
        public Grid<int> previousVisionData;
        public Grid<int> groundData;

        public Vector2 worldScale;
        
        public bool cacheVisible;

        public Vector2Int GetMatrixPosition(Vector2 p)
        {
            var w = (float) width;
            var h = (float) height;

            var i = Mathf.RoundToInt(p.x / worldScale.x + w * 0.5f);
            var j = Mathf.RoundToInt(p.y / worldScale.y + h * 0.5f);

            return new Vector2Int
            {
                x = Math.Max(0, Math.Min(i, width - 1)),
                y = Math.Max(0, Math.Min(j, height - 1))
            };
        }

        public Vector2 GetWorldPosition(int i, int j)
        {
            var w = (float) width;
            var h = (float) height;

            var x = (i - w * 0.5f) * worldScale.x;
            var y = (j - h * 0.5f) * worldScale.y;

            return new Vector2(x, y);
        }

        private bool IsBlocked(int groundLevel, int x0, int y0, int x1, int y1)
        {
            // UnityEngine.Profiling.Profiler.BeginSample("IsBlocked");

            var dx = cachedAbsoluteValues.cache[x1 - x0 + cachedAbsoluteValues.width];
            var dy = cachedAbsoluteValues.cache[y1 - y0 + cachedAbsoluteValues.width];
            
//            var dy = cachedAbsoluteValues.Abs(y1 - y0);
		
            var sx = x0 < x1 ? 1 : -1;
            var sy = y0 < y1 ? 1 : -1;

            var err = (dx > dy ? dx : -dy) / 2;
            int e2;

            var blocked = false;

            for (;;)
            {
                // Tests if current pixel is already visible by current vision,
                // that means the line to the center is clear.
//                var tmp = temporaryVisibleData.ReadValue(x0, y0);

                var tmp = temporaryVisibleData.values[x0 + y0 * temporaryVisibleData.width];
                if (cacheVisible && tmp == 2)
                {
                    break;
                }

                var ground = groundData.values[x0 + y0 * groundData.width];
                
//                var ground = groundData.ReadValue(x0, y0);
//                var ground = visionMatrix.ground[x0 + y0 * width];

                if (ground > groundLevel)
                {
                    blocked = true;
                    break;
                }
			
                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;
			
                if (e2 > -dx)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 >= dy) 
                    continue;
			
                err += dx;
                y0 += sy;
            }

//            UnityEngine.Profiling.Profiler.EndSample();
            
            return blocked;
        }
        
        private bool IsLineOfSightBlocked(int playerFlags, short groundLevel, int x0, int y0, int x1, int y1)
        {
            if (!raycastEnabled)
                return false;
                    
            // Profiler.BeginSample("CheckLineOfSight");

            var dx = cachedAbsoluteValues.Abs(x1 - x0);
            var dy = cachedAbsoluteValues.Abs(y1 - y0);
		
            var sx = x0 < x1 ? 1 : -1;
            var sy = y0 < y1 ? 1 : -1;

            var err = (dx > dy ? dx : -dy) / 2;
            int e2;

            var blocked = false;

            var maxLevel = -1;
            
            for (;;)
            {
                var ground = groundData.ReadValue(x0, y0);
                
                if (ground < maxLevel)
                {
                    blocked = true;
                    break;
                }

                if (ground > groundLevel) {
                    maxLevel = Mathf.Max(maxLevel, ground);
                }

                var players = visionData.ReadValue(x0, y0);
                
                if ((players & playerFlags) == 0)
                {
                    blocked = true;
                    break;
                }
			
                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;
			
                if (e2 > -dx)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 >= dy) 
                    continue;
			
                err += dx;
                y0 += sy;
            }

            // Profiler.EndSample();
            return blocked;
        }
	
        private void DrawPixel(int player, int x0, int y0, int x, int y, int groundLevel)
        {
            if (!visionData.IsInside(x, y))
                return;

            var blocked = false;
		
            // 0 means not visited yet
            // 1 means blocked
            // 2 means not blocked and visited

            if (raycastEnabled)
            {
                // return (values[i + j * width] & value) > 0;

                var isValue = (visionData.values[x + y * visionData.height] & player) > 0;
                
                // Avoid recalculating this pixel blocked if was already visible by the player 
                if (isValue)
                    return;
			
                if (cacheVisible)
                {
                    temporaryVisibleData.StoreValue(1, x, y);
                }
			
                blocked = IsBlocked(groundLevel, x, y, x0, y0);
            }
		
            if (blocked)
            {
                return;
            } 
		
            if (raycastEnabled && cacheVisible)
            {
                temporaryVisibleData.StoreValue(2, x, y);
            }
		
            visionData.values[x + y * visionData.width] |= player;
            previousVisionData.values[x + y * previousVisionData.width] |= player;
            

        }

        public void Clear()
        {
            visionData.Clear();
            previousVisionData.Clear();
        }

        public void ClearVision()
        {
            visionData.Clear();
        }

        public void UpdateVisions(List<VisionData> visions)
        {
            for (var j = 0; j < visions.Count; j++)
            {
                var vision = visions[j];
                var mp = GetMatrixPosition(vision.position);
                
                if (raycastEnabled && cacheVisible)
                {
                    temporaryVisibleData.Clear();
                }
		
                var radius = Mathf.FloorToInt(vision.range / worldScale.x) - 1;

                if (radius <= 0)
                    return;
            
                var x0 = mp.x;
                var y0 = mp.y;
		
                var x = radius;
                var y = 0;
                var xChange = 1 - (radius << 1);
                var yChange = 0;
                var radiusError = 0;
		
                while (x >= y)
                {
                    for (var i = x0 - x; i <= x0 + x; i++)
                    {
                        DrawPixel(vision.player, x0, y0, i, y0 + y, vision.groundLevel);
                        DrawPixel(vision.player, x0, y0, i, y0 - y, vision.groundLevel);
                    }
                    for (var i = x0 - y; i <= x0 + y; i++)
                    {
                        DrawPixel(vision.player, x0, y0, i, y0 + x, vision.groundLevel);
                        DrawPixel(vision.player, x0, y0, i, y0 - x, vision.groundLevel);
                    }

                    y++;
                    radiusError += yChange;
                    yChange += 2;
			
                    if (((radiusError << 1) + xChange) > 0)
                    {
                        x--;
                        radiusError += xChange;
                        xChange += 2;
                    }
                }
            }
        }
        
        // public void RegisterObstacle(VisionObstacle obstacle)
        // {
        //     for (var i = 0; i < width; i++)
        //     {
        //         for (var j = 0; j < height; j++)
        //         {
        //             var p = GetWorldPosition(i, j);
        //             var currentLevel = groundData.ReadValue(i, j);
					   //
        //             if (currentLevel < obstacle.GetGroundLevel(p))
        //                 currentLevel = obstacle.GetGroundLevel(p);
        //
        //             groundData.StoreValue(currentLevel, i, j);
        //         }
        //     }		
        // }
        
        public void RegisterObstacle(Vector2 position, int groundLevel)
        {
            var matrixPosition = GetMatrixPosition(position);
            var currentGroundLevel = groundData.ReadValue(matrixPosition.x, matrixPosition.y);
            if (groundLevel > currentGroundLevel)
            {
                groundData.StoreValue(groundLevel, matrixPosition.x, matrixPosition.y);
            }
        }
	
        public int GetGroundLevel(Vector3 position)
        {
            var mp = GetMatrixPosition(position);
            return groundData.ReadValue(mp.x, mp.y);
        }


        public bool IsLineOfSightBlocked(int playerFlags, short groundLevel, Vector3 p0, Vector3 p1)
        {
            var m0 = GetMatrixPosition(p0);
            var m1 = GetMatrixPosition(p1);

            return IsLineOfSightBlocked(playerFlags, groundLevel, m0.x, m0.y, m1.x, m1.y);
        }

        public bool IsVisible(int playerFlags, Vector2 position)
        {
            return IsVisible(playerFlags,  GetMatrixPosition(position));
        }
        
        public bool IsVisible(int playerFlags, Vector2Int position)
        {
            var visible = visionData.IsValue(playerFlags, position.x, position.y);
            return visible;
        }

        public int GetPlayersVision(Vector3 worldPosition)
        {
            var p = GetMatrixPosition(worldPosition);
            if (!visionData.IsInside(p.x, p.y))
                return 0;
            return visionData.ReadValue(p.x, p.y);
        }
    }
}