using NUnit.Framework;
using UnityEngine;
using Assert = NUnit.Framework.Assert;

namespace Gemserk.DataGrids.Tests
{
    public class WorldGridTests
    {
        [Test]
        public void WorldGrid_Convert_NonDivisibleBy2()
        {
            var width = 47;
            
            var worldGridData = new WorldGridData(new Vector2(width, 1), new Vector2(0.75f, 1));

            var current = 0;
            var p = -width * 0.5f;
            
            for (int i = 0; i < width; i++)
            {
                Assert.AreEqual(current, worldGridData.GetGridPosition(new Vector2(p, 0)).x, $"for object {i}");
                current++;
                p += 0.75f;
            }

        }
        
        [Test]
        public void WorldGrid_Convert_BetterNumbersWorld()
        {
            var width = 100;
            var worldGridData = new WorldGridData(new Vector2(width, 1), new Vector2(1, 1));

            var current = 0;
            var p = -50f;
            
            for (int i = 0; i < width; i++)
            {
                Assert.AreEqual(current, worldGridData.GetGridPosition(new Vector2(p, 0)).x, $"for object {i}");
                current++;
                p += 1;
            }

        }
        
        [Test]
        public void WorldGrid_CreateGrid_HasProperWorldSize()
        {
            var worldGridData = new WorldGridData(new Vector2(47, 47), new Vector2(0.75f, 0.75f));

            var createdGrid = worldGridData.CreateGrid(10);
            Assert.AreEqual(worldGridData.width, createdGrid.width);
            Assert.AreEqual(worldGridData.height, createdGrid.height);
        }
        
        [Test]
        public void WorldGrid_ClampPositionInsideWorld()
        {
            var worldGridData = new WorldGridData(new Vector2(50, 20), new Vector2(1, 1));
            var position = worldGridData.ClampPositionInsideWorld(new Vector2(-100, -100));
            
            Assert.AreEqual(-25f, position.x, 1f);
            Assert.AreEqual(-10f, position.y, 1f);
            
            position = worldGridData.ClampPositionInsideWorld(new Vector2(100, 100));
            
            Assert.AreEqual(25f, position.x, 1f);
            Assert.AreEqual(10f, position.y, 1f);
        }
        
    }
}

