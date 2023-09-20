using Game.Definitions;
using Game.Development;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct HitBox
    {
        public Vector3 position;
        public Vector3 offset;
        public Vector3 size;

        public Vector3 position3d => position + offset;

        public static HitBox AllTheWorld => new HitBox()
        {
            position = Vector2.zero,
            offset = Vector2.zero,
            size = Vector2.positiveInfinity
        };
    }
    
    public struct HitBoxComponent : IEntityComponent
    {
        public HitboxAsset defaultHurt;
        
        public HitBox hit;
        public HitBox hurt;

        public DebugHitBox debugHitBox;
        public DebugHitBox debugHurtBox;
    }
    
    public struct HurtBoxColliderComponent : IEntityComponent
    {
        public BoxCollider collider;
        public TargetReference targetReference;
    }
}