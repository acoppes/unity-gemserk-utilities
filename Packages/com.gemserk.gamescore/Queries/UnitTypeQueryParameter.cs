using Game.Components;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Queries
{
    public readonly struct UnitTypeParameter : IQueryParameter
    {
        private readonly int type;

        public UnitTypeParameter(int type)
        {
            this.type = type;
        }
        
        public bool MatchQuery(Entity entity)
        {
            if (!entity.Has<UnitTypeComponent>())
            {
                return false;
            }

            var entityType = entity.Get<UnitTypeComponent>().unitType;
            return BitMaskCheck.Match(type, entityType);
            // return type.HasUnitType(entityType);
        }
    }
    
    public class UnitTypeQueryParameter : QueryParameterBase
    {
        public BitmaskTypeAsset[] types;
        
        [BitMask(16)]
        [Tooltip("Editable it type assets list is empty, otherwise is the result of the bitmask.")]
        public int unitType;

        private void OnValidate()
        {
            if (types.Length > 0)
            {
                unitType = 0;
                foreach (var type in types) {
                    if (type != null)
                    {
                        unitType |= type.type;
                    }
                }
            }
        }

        public override bool MatchQuery(Entity entity)
        {
            return new UnitTypeParameter(unitType).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"isType({unitType})";
        }
    }
}