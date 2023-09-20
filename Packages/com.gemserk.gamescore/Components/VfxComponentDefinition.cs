using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Components
{
    [Serializable]
    public class VfxComponentDataDefinition
    {
        public VfxComponentData.PositionType positionType;
        public VfxComponentData.RandomOffsetType randomOffsetType = VfxComponentData.RandomOffsetType.None;
        
        // TODO: attachpoints here could be an asset to identify in all the places the same one
        [ConditionalField(nameof(positionType), false, VfxComponentData.PositionType.AttachPoint)]
        public string attachPoint;

        [ConditionalField(nameof(randomOffsetType), false, VfxComponentData.RandomOffsetType.PlaneXZ)]
        public float range;
        
        public Object definition;

        public VfxComponentData ToData()
        {
            return new VfxComponentData()
            {
                positionType = positionType,
                definition = definition.GetInterface<IEntityDefinition>(),
                attachPoint = attachPoint,
                randomOffsetType = randomOffsetType,
                range = range
            };
        }   
    }

    public struct VfxComponentData
    {
        public enum PositionType
        {
            Ground = 0,
            AttachPoint = 1,
        }
        
        public enum RandomOffsetType
        {
            None = 0,
            PlaneXZ = 1,
        }
        
        public PositionType positionType;
        public RandomOffsetType randomOffsetType;
        public string attachPoint;
        public Vector3 position;
        public float range;
        public IEntityDefinition definition;
    }
    
    public struct VfxComponent : IEntityComponent
    {
        // public enum FollowType
        // {
        //     InitialPosition,
        //     CurrentPosition
        // }
        
        public string animation;

        public Entity target;
        // public FollowType followType;
    }
    
    public class VfxComponentDefinition : ComponentDefinitionBase
    {

        public override string GetComponentName()
        {
            return nameof(VfxComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new VfxComponent
            {

            });
        }
    }
}