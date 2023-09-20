using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Utilities
{
    [ExecuteInEditMode]
    public class ColliderFromTilemap : MonoBehaviour
    {
        public Tilemap tilemap;
        public Transform collidersParent;


        [ButtonMethod()]
        public void Reload()
        {
            collidersParent.DestroyAllChildren();
            
            var bounds = tilemap.cellBounds;

            var t = tilemap.GetComponent<TilemapCollider2D>();
            for (int i = 0; i < t.shapeCount; i++)
            {
                var shapeGroup = new PhysicsShapeGroup2D();
                var shapes = t.GetShapes(shapeGroup, i);
                
                Debug.Log(shapes);

                for (int j = 0; j < shapes; j++)
                {
                    var shape = shapeGroup.GetShape(j);
                    Debug.Log(shape.shapeType);
                }

            }

            for (var i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (var j = bounds.yMin; j < bounds.yMax; j++)
                {
                    var position = new Vector3Int(i, j);
                    var tile = tilemap.GetTile(position);


                    if (tile != null)
                    {
                        var data = new TileData();
                        tile.GetTileData(position, tilemap, ref data);
                    
                        if (data.sprite != null)
                        {
                            Debug.Log(data.sprite.name);
                        }
                    }
                }
            }
        }
        
    }
}