﻿using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct BreakableComponent : IEntityComponent
    {
        public bool broken;
    }
    
    public class BreakableObstacleComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new BreakableComponent());
        }
    }
}