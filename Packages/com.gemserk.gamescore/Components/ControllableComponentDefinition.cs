using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Serialization;

namespace Game.Components
{
    public struct ControllableByComponent : IEntityComponent
    {
        [Flags]
        public enum ControllableType
        {
            Nothing = 0,
            Player = 1 << 0,
            AI = 1 << 1,
            Everything = -1
        }

        public ControllableType controllableType;

        public int playerControlId;

        public bool IsControllableByAI()
        {
            return controllableType.HasControllableType(ControllableType.AI);
        }
        
        public bool IsControllableByPlayer()
        {
            return controllableType.HasControllableType(ControllableType.Player);
        }
    }
    
    public class ControllableComponentDefinition : ComponentDefinitionBase
    {
        public ControllableByComponent.ControllableType controllableType;
        public int playerControlId;
        
        public override string GetComponentName()
        {
            return nameof(ControllableByComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ControllableByComponent()
            {
                controllableType = controllableType,
                playerControlId = playerControlId
            });
        }
    }
}