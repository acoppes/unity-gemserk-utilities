using System.Collections.Generic;
using Game.Components;
using Game.Components.Abilities;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class AbilitiesSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AbilitiesComponent>, Exc<DisabledComponent>> abilitiesFilter = default;
        readonly EcsFilterInject<Inc<AbilitiesComponent, PositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        readonly EcsFilterInject<Inc<AbilitiesComponent, LookingDirection>, Exc<DisabledComponent>> directionFilter = default;
        readonly EcsFilterInject<Inc<AbilitiesComponent, PlayerComponent>, Exc<DisabledComponent>> playerFilter = default;
        
        readonly EcsFilterInject<Inc<TargetComponent>, Exc<DisabledComponent>> targetsFilter = default;
        
        // var ecsFilter = world.GetFilter<TargetComponent>().Exc<DisabledComponent>().End();
        
        private readonly List<Target> temporaryTargets = new List<Target>();
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in positionFilter.Value)
            {
                ref var abilities = ref positionFilter.Pools.Inc1.Get(entity);
                var position = positionFilter.Pools.Inc2.Get(entity);

                foreach (var ability in abilities.abilities)
                {
                    ability.center = position.value;
                }
            }
            
            foreach (var entity in directionFilter.Value)
            {
                ref var abilities = ref directionFilter.Pools.Inc1.Get(entity);
                var lookingDirection = directionFilter.Pools.Inc2.Get(entity);

                foreach (var ability in abilities.abilities)
                {
                    ability.direction = lookingDirection.value;
                }
            }
            
            foreach (var entity in playerFilter.Value)
            {
                ref var abilities = ref playerFilter.Pools.Inc1.Get(entity);
                var player = playerFilter.Pools.Inc2.Get(entity);

                foreach (var ability in abilities.abilities)
                {
                    ability.player = player.player;
                }
            }

            foreach (var entity in abilitiesFilter.Value)
            {
                ref var abilitiesComponent = ref abilitiesFilter.Pools.Inc1.Get(entity);
                abilitiesComponent.hasExecutingAbility = false;
                
                foreach (var ability in abilitiesComponent.abilities)
                {
                    if (ability.isExecuting)
                    {
                        abilitiesComponent.hasExecutingAbility = true;
                        break;
                    }
                }

                foreach (var ability in abilitiesComponent.abilities)
                {
                    var reloadCooldown = false;

                    if (ability.isExecuting)
                    {
                        ability.executionTime += dt;
                    }
                    
                    if (ability.resetCooldownType == Ability.ResetCooldownType.ResetsIfAnyActionExecuting && abilitiesComponent.hasExecutingAbility)
                    {
                        ability.cooldown.Reset();
                    }
                    
                    if (ability.cooldownType == Ability.ReloadCooldownType.IfNoExecuting && !ability.isExecuting)
                    {
                        reloadCooldown = true;
                    } 
                    
                    if (!abilitiesComponent.hasExecutingAbility &&
                               ability.cooldownType == Ability.ReloadCooldownType.IfNoActionsExecuting)
                    {
                        reloadCooldown = true;
                    }

                    if (ability.cooldownType == Ability.ReloadCooldownType.Disabled)
                    {
                        reloadCooldown = false;
                    }
                    
                    if (reloadCooldown)
                    {
                        ability.cooldown.Increase(dt);
                    }

                    if (ability.autoTarget && !ability.targetsLocked)
                    {
                        temporaryTargets.Clear();
                        var runtimeTargetingParameters = ability.GetRuntimeTargetingParameters();
                        
                        foreach (var targetEntity in targetsFilter.Value)
                        {
                            var targetComponent = targetsFilter.Pools.Inc1.Get(targetEntity);
                            if (TargetingUtils.ValidateTarget(targetComponent.target, runtimeTargetingParameters))
                            {
                                temporaryTargets.Add(targetComponent.target);
                            }
                        }
                        
                        // world.GetTargetsFromHitboxes(ability.GetRuntimeTargetingParameters(), temporaryTargets);
                        
                        if (runtimeTargetingParameters.filter.sorter is ITargetSorter sorter)
                        {
                            sorter.Sort(temporaryTargets, runtimeTargetingParameters);
                        }
                        
                        ability.CopyTargets(temporaryTargets);
                    }

                    if (ability.targetsLocked)
                    {
                        for (var i = 0; i < ability.abilityTargets.Count; i++)
                        {
                            var abilityTarget = ability.abilityTargets[i];
                                
                            if (abilityTarget.target == null)
                            {
                                continue;
                            }

                            abilityTarget.valid = ability.IsValidTarget(abilityTarget.target);
                            // while target is valid, update position of ability target.
                            if (abilityTarget.valid)
                            {
                                abilityTarget.position = abilityTarget.target.position;
                            }
                            
                            ability.abilityTargets[i] = abilityTarget;
                        }
                    }
                }
            }
        }
    }
}