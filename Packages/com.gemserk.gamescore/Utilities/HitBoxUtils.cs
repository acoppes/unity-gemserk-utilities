using System.Runtime.CompilerServices;
using Game.Components;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    public static class HitBoxUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HitBox GetHitBox(this HitboxAsset hitBoxAsset, PositionComponent position, LookingDirection lookingDirection)
        {
            return GetHitBox(hitBoxAsset, position.value, lookingDirection.value);
        }
        
        public static HitBox GetHitBox(this HitboxAsset hitBoxAsset, Vector3 position, Vector3 lookingDirection)
        {
            var offset = hitBoxAsset.offset;
                    
            if (lookingDirection.x < 0)
            {
                offset.x *= -1;
            }
                    
            return new HitBox
            {
                size = hitBoxAsset.size,
                position = position,
                offset = offset
            };
        }
    }
}