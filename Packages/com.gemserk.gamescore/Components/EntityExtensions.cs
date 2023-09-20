using System.Runtime.CompilerServices;
using Game.Components.Abilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;

namespace Game.Components
{
    public static class EntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref PointsComponent GetPointsComponent(this Entity entity)
        {
            return ref entity.Get<PointsComponent>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref JumpComponent GetJumpComponent(this Entity entity)
        {
            return ref entity.Get<JumpComponent>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref PlatformerComponent GetPlatformerComponent(this Entity entity)
        {
            return ref entity.Get<PlatformerComponent>();
        }
                
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref DashComponent GetDashComponent(this Entity entity)
        {
            return ref entity.Get<DashComponent>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref PickupComponent GetPickupComponent(this Entity entity)
        {
            return ref entity.Get<PickupComponent>();
        }
        
        public static ref AbilitiesComponent GetAbilitiesComponent(this Entity entity)
        {
            return ref entity.Get<AbilitiesComponent>();
        }
        
        public static ref InputComponent GetControlComponent(this Entity entity)
        {
            return ref entity.Get<InputComponent>();
        }
        
        public static ref HealthComponent GetHealthComponent(this Entity entity)
        {
            return ref entity.Get<HealthComponent>();
        }
        
        public static ref StatesComponent GetStatesComponent(this Entity entity)
        {
            return ref entity.Get<StatesComponent>();
        }
        
        public static ref ActiveControllerComponent GetActiveControllerComponent(this Entity entity)
        {
            return ref entity.Get<ActiveControllerComponent>();
        }
        
        public static ref DestroyableComponent GetDestroyableComponent(this Entity entity)
        {
            return ref entity.Get<DestroyableComponent>();
        }
        
        public static ref PlayerComponent GetPlayerComponent(this Entity entity)
        {
            return ref entity.Get<PlayerComponent>();
        }

        public static ref PositionComponent GetPositionComponent(this Entity entity)
        {
            return ref entity.Get<PositionComponent>();
        }

        public static ref LookingDirection GetLookingDirectionComponent(this Entity entity)
        {
            return ref entity.Get<LookingDirection>();
        }
        
        public static ref PhysicsComponent GetPhysicsComponent(this Entity entity)
        {
            return ref entity.Get<PhysicsComponent>();
        }
        
        public static ref Physics2dComponent GetPhysics2dComponent(this Entity entity)
        {
            return ref entity.Get<Physics2dComponent>();
        }
    }
}