using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct PointsComponent : IEntityComponent
    {
        public float points;

        public int combo;
        public int maxCombo;
        public float multiplierPerCombo;
    }
    
    public class PointsComponentDefinition : ComponentDefinitionBase
    {
        public float startingPoints = 0;

        public int maxCombo = 15;
        public float multiplierPerCombo = 10;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PointsComponent()
            {
                points = startingPoints,
                maxCombo = maxCombo,
                multiplierPerCombo = multiplierPerCombo
            });
        }
    }
}