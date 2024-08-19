using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct SelectionComponent : IEntityComponent
    {
        public bool isMain;
        
        public List<Entity> selectingEntities;
        public List<Entity> selectedEntities;
        
        public bool isSelecting;
        public Vector3 start, end;

        public Bounds bounds
        {
            get
            {
                var center = (start + end) / 2;
                // var size = end - start;
                return new Bounds(center, new Vector3(
                    Mathf.Abs(end.x - start.x), 
                    Mathf.Abs(end.y - start.y), 
                    Mathf.Abs(end.z - start.z)
                    ));
            }
        }
    }
    
    public class SelectionComponentDefinition : ComponentDefinitionBase
    {
        public bool isMain;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new SelectionComponent()
            {
                isMain = isMain,
                selectedEntities = new List<Entity>(),
                selectingEntities = new List<Entity>()
            });
        }
    }
}