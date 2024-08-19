using Game.Models;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct HasShadowComponent : IEntityComponent
    {
        public enum CopyFromPositionType
        {
            FromFake3d = 0,
            From2d = 1,
            Manual = 2,
            From3d = 3
        }
        
        public GameObject prefab;
        public Shadow instance;
        public float shadowPerspective;
        public bool copyFromModel;
        public Vector3 position;

        public float distanceToGround;
        
        public CopyFromPositionType copyFromPositionType;

        public GameObject shadowDefintion;
        public Entity shadowEntity;
    }
    
    public class HasShadowComponentDefinition : ComponentDefinitionBase
    {
        public GameObject shadowCustomPrefab;
        public float shadowPerspective = 0.2f;
        
        // used to copy from model to model
        public bool shadowCopyFromModel = false;
        
        public HasShadowComponent.CopyFromPositionType copyFromPositionType = HasShadowComponent.CopyFromPositionType.FromFake3d;

        public GameObject shadowDefinitionPrefab;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HasShadowComponent()
            {
                prefab = shadowCustomPrefab,
                shadowPerspective = shadowPerspective,
                copyFromModel = shadowCopyFromModel,
                copyFromPositionType = copyFromPositionType,
                distanceToGround = 0,
                shadowDefintion = shadowDefinitionPrefab
            });
        }
    }
}