using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Components;
using Game.Components.Abilities;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Utilities
{
    public static class TargetingUtils
    {
        public static bool HasAliveFlag(this HealthComponent.AliveType self, HealthComponent.AliveType flag)
        {
            return (self & flag) == flag;
        }

        // fixed targeting parameters (like what I am interested in targeting)

        // runtime values (like current player, position, etc)

        public class DistanceComparer : Comparer<Target>
        {
            private Vector3 position;

            public DistanceComparer(Vector3 position)
            {
                this.position = position;
            }
            
            public override int Compare(Target x, Target y)
            {
                var aDifference = x.position - position;
                var bDifference = y.position - position;

                if (aDifference.sqrMagnitude < bDifference.sqrMagnitude)
                {
                    return -1;
                }

                if (aDifference.sqrMagnitude > bDifference.sqrMagnitude)
                {
                    return 1;
                }

                return 0;
            }
        }
        
        public class DistanceLineComparer : Comparer<Target>
        {
            private Vector3 position;

            public DistanceLineComparer(Vector3 position)
            {
                this.position = position;
            }
            
            public override int Compare(Target x, Target y)
            {
                var aDifference = x.position - position;
                var bDifference = y.position - position;

                if (Mathf.Abs(aDifference.z) < Mathf.Abs(bDifference.z))
                {
                    return -1;
                }

                if (Mathf.Abs(aDifference.z) > Mathf.Abs(bDifference.z))
                {
                    return 1;
                }

                return 0;
            }
        }
        
        public class DirectionAlignedComparer : Comparer<Target>
        {
            private Vector3 position;
            private Vector2 direction;

            public DirectionAlignedComparer(Vector3 position, Vector2 direction)
            {
                this.position = position;
                this.direction = direction;
            }
            
            public override int Compare(Target x, Target y)
            {
                var aDifference = x.position - position;
                var bDifference = y.position - position;

                var xAngle = Vector2.Angle(direction, aDifference.XZ());
                var yAngle = Vector2.Angle(direction, bDifference.XZ());

                if (xAngle < yAngle)
                {
                    return -1;
                }

                if (yAngle < xAngle)
                {
                    return 1;
                }

                return 0;
            }
        }

        private static readonly ContactFilter2D HurtBoxContactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("HurtBox")
        };
        
        private static readonly Collider[] colliders = new Collider[20];

        private static readonly List<Target> tempTargets = new List<Target>();
        
        // private static readonly Target[] targets = new Target[20];

        public static List<Target> GetTargetsUsingHitbox(this World world, Entity source)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            var player = world.GetComponent<PlayerComponent>(source);
            
            return world.GetTargetsFromHitboxes(hitBox.hit, new RuntimeTargetingParameters
            {
                alliedPlayersBitmask = PlayerAllianceExtensions.GetAlliedPlayers(player.player),
                filter = new TargetingFilter
                {
                    // area = hitBox.hit,
                    playerAllianceType = PlayerAllianceType.Enemies,
                    // areaType = TargetingFilter.AreaType.HitBox,
                    aliveType = HealthComponent.AliveType.Alive
                }
            });
        }

        public static Target GetFirstTarget(this World world, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            tempTargets.Clear();
            if (GetTargets(world, runtimeTargetingParameters, tempTargets, 1) > 0)
            {
                return tempTargets[0];
            }
            return null;
        }
        

        public static List<Target> GetTargetsFromHitboxes(this World world, HitBox area, RuntimeTargetingParameters runtimeTargetingParameters, int maxTargets = 0)
        {
            var results = new List<Target>();

            var filter = runtimeTargetingParameters.filter;
            var targetsCount = 0;
            
            // if (filter.areaType == TargetingFilter.AreaType.HitBox)
            // {
            //     // collect targets using physics collider

            // } else
            // {
            //
            // }
            
            var colliderCount = DrawPhysics.OverlapBoxNonAlloc(area.position3d, area.size * 0.5f, colliders,
                Quaternion.identity, HurtBoxContactFilter.layerMask,
                QueryTriggerInteraction.Collide);
            
            if (colliderCount > 0)
            {
                for (var i = 0; i < colliderCount; i++)
                {
                    if (maxTargets > 0 && targetsCount >= maxTargets)
                        break;
                    
                    var collider = colliders[i];
                    var targetEntityReference = collider.GetComponent<TargetReference>();
                    var target = targetEntityReference.target;
                    
                    if (ValidateTarget(target, runtimeTargetingParameters))
                    {
                        results.Add(target);
                        targetsCount++;
                    }
                }
            }
            
            // filter valid targets
            // foreach (var target in targets)
            // {
            //     if (ValidateTarget(target, runtimeTargetingParameters))
            //     {
            //         results.Add(target);
            //     }
            // }

            if (runtimeTargetingParameters.filter.sorter is ITargetSorter sorter)
            {
                sorter.Sort(results, runtimeTargetingParameters);
            }

            // return targetsCount;
            
            return results;
        }
        
        public static List<Target> GetTargetsNoHitbox(this World world, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            var results = new List<Target>();
            GetTargets(world, runtimeTargetingParameters, results);
            return results;
        }
        
        public static int GetTargets(this World world, RuntimeTargetingParameters runtimeTargetingParameters, List<Target> results, int maxTargets = 0)
        {
           // var targets = new List<Target>();

            var filter = runtimeTargetingParameters.filter;
            var targetsCount = 0;
            
            // if (filter.areaType == TargetingFilter.AreaType.HitBox)
            // {
            //     // collect targets using physics collider
            //     var area = filter.area;
            //
            //     var colliderCount = DrawPhysics.OverlapBoxNonAlloc(area.position3d, area.size * 0.5f, colliders,
            //         Quaternion.identity, HurtBoxContactFilter.layerMask,
            //         QueryTriggerInteraction.Collide);
            //
            //     if (colliderCount > 0)
            //     {
            //         for (var i = 0; i < colliderCount; i++)
            //         {
            //             if (maxTargets > 0 && targetsCount >= maxTargets)
            //                 break;
            //             
            //             var collider = colliders[i];
            //             var targetEntityReference = collider.GetComponent<TargetReference>();
            //             var target = targetEntityReference.target;
            //             
            //             if (ValidateTarget(target, runtimeTargetingParameters))
            //             {
            //                 results.Add(target);
            //                 targetsCount++;
            //             }
            //         }
            //     }
            // } else
            // {
            //
            // }
            
            var targetComponents = world.GetComponents<TargetComponent>();

            // TODO: cache filter or move it to system
            var ecsFilter = world.GetFilter<TargetComponent>().Exc<DisabledComponent>().End();
                
            foreach (var entity in ecsFilter)
            {
                if (maxTargets > 0 && targetsCount >= maxTargets)
                    break;
                    
                var targetComponent = targetComponents.Get(entity);
                if (ValidateTarget(targetComponent.target, runtimeTargetingParameters))
                {
                    results.Add(targetComponent.target);
                    targetsCount++;
                }
            }
            
            // filter valid targets
            // foreach (var target in targets)
            // {
            //     if (ValidateTarget(target, runtimeTargetingParameters))
            //     {
            //         results.Add(target);
            //     }
            // }

            if (runtimeTargetingParameters.filter.sorter is ITargetSorter sorter)
            {
                sorter.Sort(results, runtimeTargetingParameters);
            }

            return targetsCount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateTarget(Target target, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            if (target == null)
            {
                return false;
            }
            
            var targetingFilter = runtimeTargetingParameters.filter;

            var targetTypes = targetingFilter.targetTypes;
            
            if (targetTypes != 0 && !targetTypes.HasTargetFlag(target.targetType))
            {
                return false;
            }
            
            if (targetingFilter.playerAllianceType != PlayerAllianceType.Everything)
            {
                var targetPlayerBitmask = 1 << target.player;
                var allies = (runtimeTargetingParameters.alliedPlayersBitmask & targetPlayerBitmask) != 0;

                var allianceCheck = (allies && targetingFilter.playerAllianceType == PlayerAllianceType.Allies) || (
                    !allies && targetingFilter.playerAllianceType == PlayerAllianceType.Enemies);

                if (!allianceCheck)
                {
                    return false;
                }
            }
            
            // if (!targetingFilter.playerAllianceType.CheckPlayerAlliance(runtimeTargetingParameters.alliedPlayersBitmask, target.playerBitmask))
            // {
            //     return false;
            // }
            
            if (targetingFilter.aliveType != HealthComponent.AliveType.None)
            {
                if (target.aliveType == HealthComponent.AliveType.None)
                {
                    return false;
                }

                if ((targetingFilter.aliveType & target.aliveType) == 0)
                    return false;
                
                // if (!targetingFilter.aliveType.HasFlag(target.aliveType))
                // {
                //     return false;
                // }
            }

            var difference = target.position - runtimeTargetingParameters.position;

            if (targetingFilter.angle.Min > 0 || targetingFilter.angle.Max < 180)
            {
                var targetAngle = Vector2.Angle(runtimeTargetingParameters.direction.XZ(), difference.XZ());
                if (targetAngle > targetingFilter.angle.Max)
                {
                    return false;
                }
                
                if (targetAngle < targetingFilter.angle.Min)
                {
                    return false;
                }
            }

            if (targetingFilter.distanceType == TargetingFilter.CheckDistanceType.InsideDistance)
            {
                var differenceSqrMagnitude = difference.sqrMagnitude;
                
                if (differenceSqrMagnitude > targetingFilter.maxRangeSqr)
                {
                    return false;
                }
                
                if (differenceSqrMagnitude < targetingFilter.minRangeSqr)
                {
                    return false;
                }
            }
            
            if (targetingFilter.distanceType == TargetingFilter.CheckDistanceType.InsideDistanceXZ)
            {
                var differenceSqrMagnitude = difference.XZ().sqrMagnitude;
                
                if (differenceSqrMagnitude > targetingFilter.maxRangeSqr)
                {
                    return false;
                }
                
                if (differenceSqrMagnitude < targetingFilter.minRangeSqr)
                {
                    return false;
                }
            }

            if (targetingFilter.customFilter is ITargetCustomFilter customFilter)
            {
                if (!customFilter.Filter(target, runtimeTargetingParameters))
                {
                    return false;
                }
            }

            return true;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RuntimeTargetingParameters GetRuntimeTargetingParameters(this Ability ability)
        {
            return new RuntimeTargetingParameters()
            {
                filter = ability.targeting.targetingFilter,
                position = ability.center,
                direction = ability.direction,
                alliedPlayersBitmask = ability.alliedPlayersBitmask,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidTarget(this Ability ability, Target target)
        {
            return ValidateTarget(target, ability.GetRuntimeTargetingParameters());
        }

        public static Vector3 AdjustDirectionToNearestEnemyInLine(World world, int player, Vector3 position, Vector3 lookingDirection, 
            float lineAngle, float lineHeight)
        {
            // Search for enemies in line or angle and adjust direction
            var targets = TargetingUtils.GetTargetsNoHitbox(world, new RuntimeTargetingParameters()
            {
                alliedPlayersBitmask = PlayerAllianceExtensions.GetAlliedPlayers(player),
                filter = new TargetingFilter()
                {
                    // area = HitBox.AllTheWorld,
                    aliveType = HealthComponent.AliveType.Alive,
                    // areaType = TargetingFilter.AreaType.Nothing,
                    playerAllianceType = PlayerAllianceType.Enemies
                }
            });

            targets.Sort(new DistanceLineComparer(position));

            foreach (var target in targets)
            {
                var difference = target.position - position;

                if (Vector2.Angle(lookingDirection.XZ(), difference.XZ()) > lineAngle)
                {
                    continue;
                }

                if (Mathf.Abs(difference.z) > lineHeight)
                {
                    continue;
                }

                var direction = target.position - position;
                direction.Normalize();

                return direction;
            }

            return lookingDirection;
        }
    }
}