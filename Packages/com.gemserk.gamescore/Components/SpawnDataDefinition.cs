using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Components
{
    [Serializable]
    public class SpawnDataDefinition
    {
        public SpawnData.PositionType positionType;
        public SpawnData.RandomOffsetType randomOffsetType = SpawnData.RandomOffsetType.None;
        
        // TODO: attachpoints here could be an asset to identify in all the places the same one
        [ConditionalField(nameof(positionType), false, SpawnData.PositionType.AttachPoint)]
        public string attachPoint;

        [ConditionalField(nameof(randomOffsetType), false, SpawnData.RandomOffsetType.PlaneXZ)]
        public float range;
        
        [ObjectType(typeof(IEntityDefinition), filterString = "Definition", assetReferencesOnWhenStart = false, prefabReferencesOnWhenStart = true)]
        public Object definition;

        public bool disabled;
        
        public SpawnData ToData()
        {
            return new SpawnData()
            {
                positionType = positionType,
                definition = definition.GetInterface<IEntityDefinition>(),
                attachPoint = attachPoint,
                randomOffsetType = randomOffsetType,
                range = range,
                disabled = disabled
            };
        }   
    }
    
    public struct SpawnData
    {
        public enum PositionType
        {
            Ground = 0,
            AttachPoint = 1,
            Center = 2
        }
        
        public enum RandomOffsetType
        {
            None = 0,
            PlaneXZ = 1,
        }

        public bool disabled;
        public PositionType positionType;
        public RandomOffsetType randomOffsetType;
        public string attachPoint;
        public Vector3 position;
        public float range;
        public IEntityDefinition definition;
    }
}