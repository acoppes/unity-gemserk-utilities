using System.Runtime.CompilerServices;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public struct NameComponent : IEntityComponent
    {
        public string name;
        
        public bool singleton;
        public bool cachedInSingletonsDictionary;
    }
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector3 value;
        public float angle;

        public void SetFromAngle(float angleInDegrees)
        {
            value = Vector2.right.Rotate(angleInDegrees * Mathf.Deg2Rad);
        }
    }
    
    public struct PositionComponent : IEntityComponent
    {
        public Vector3 value;
        public int type;
    }
    
    public struct StartingPositionComponent : IEntityComponent
    {
        public Vector3 value;
    }
    
    public struct StartingPositionParameter : IEntityInstanceParameter
    {
        public Vector3 value;
        
        public void Apply(World world, Entity entity)
        {
            entity.AddOrSet(new StartingPositionComponent()
            {
                value = value
            });
        }
    }

    public struct PlayerComponent : IEntityComponent
    {
        public const int MaxPlayers = 8;
        
        public const int NoPlayer = -1;
        
        // will be a flag in the future
        public int player;
        public int playerBitmask => 1 << player;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetPlayerBitmask(int player)
        {
            return 1 << player;
        }
    }

    public struct DisabledComponent : IEntityComponent
    {
        
    }
    
    public struct EnableDisabledComponent : IEntityComponent
    {
        
    }

    public struct EcsWorldEntitiesDebugComponent : IEntityComponent
    {
        public string name;
        public Vector2 scrollPosition;
        public bool selected;
        public bool isSingletonByName;
    }
    
    public struct SourceEntityComponent : IEntityComponent
    {
        public Entity source;
        // copy stuff from source entity, by default all? 
    }
}