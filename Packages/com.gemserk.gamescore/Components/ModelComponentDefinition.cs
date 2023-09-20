
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;

namespace Game.Components
{
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
        
        public override string GetComponentName()
        {
            return nameof(ModelComponent);
        }

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
                flipUseRotation = flipUseRotation
            });
        }
    }
}