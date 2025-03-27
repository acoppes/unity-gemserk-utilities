
using Game.Models;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Components
{
    public struct ModelStaticProcessedComponent : IEntityComponent
    {
        
    }
    
    public struct ModelEnabledComponent : IEntityComponent
    {
        
    }

    public struct ModelSortingGroupComponent : IEntityComponent
    {
        public SortingGroup sortingGroup;
        public int order;
        public int layer;
        // for now it just updates once
        public bool updated;
    }

    public struct ModelComponent : IEntityComponent
    {
        public enum Visiblity
        {
            Visible = 0,
            Hidden = 1
        }
        
        public enum RotationType
        {
            FlipToLookingDirection = 0,
            Rotate = 1,
            None = 2
        }
        
        public enum SortingLayerType
        {
            None = 0,
            CopyFromComponent = 1
        }
        
        public GameObject prefab;
        
        public Model instance;
        public GameObject modelGameObject;
        public bool isModelActive;

        // TODO: cache this on creation
        public bool hasSubModelObject;

        public RotationType rotation;
        public bool flipUseRotation;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;

        public Color color;

        public SortingLayerType sortingLayerType;
        public int sortingOrder;
        public int sortingLayer;
        public bool sortingUpdated;

        public float fixedRotationAngles;
    }
    
    public class ModelComponentDefinition : ComponentDefinitionBase
    {
        public GameObject prefab;
        public ModelComponent.RotationType rotationType = ModelComponent.RotationType.FlipToLookingDirection;
        
        [ConditionalField(nameof(rotationType), false, ModelComponent.RotationType.FlipToLookingDirection)]
        public bool flipUseRotation = false;
        
        public Color startingColor = Color.white;
        public ModelComponent.SortingLayerType sortingLayerType = ModelComponent.SortingLayerType.None;
        public int sortingOrder;
        public string sortingLayer;

        [Tooltip("Fix final model rotation angle to rotate in partitions, use 0 or less to disable")]
        public float fixedRotationAngles = 0;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ModelComponent
            {
                prefab = prefab,
                color = startingColor,
                rotation = rotationType,
                sortingLayerType = sortingLayerType,
                sortingLayer = SortingLayer.NameToID(sortingLayer),
                sortingOrder = sortingOrder,
                flipUseRotation = flipUseRotation,
                fixedRotationAngles = fixedRotationAngles
            });
        }
    }
}