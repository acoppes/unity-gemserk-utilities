using System;
using System.Runtime.CompilerServices;
using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class HealthSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public static bool DebugLogDamages;
        
        readonly EcsFilterInject<Inc<HealthComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<HealthComponent, HealthPlayerTeamDamageMultiplier, PlayerComponent>, Exc<DisabledComponent>> damagePerTeamFilter = default;
        readonly EcsFilterInject<Inc<HealthStateChangedEvent>> stateChangedFilter = default;
        
        public static readonly float[] DamageMultipliersPerTeam = new float[PlayerComponent.MaxPlayers];
        
        public void Init(EcsSystems systems)
        {
            Array.Fill(DamageMultipliersPerTeam, 1f);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HealthChangeData ProcessDamage(ref HealthComponent health, HealthChangeData healthChange, float teamDamageMultiplier = 1f)
        {
            var resistanceMultiplier = Mathf.Clamp01(1f - health.damageResistance);
            healthChange.value *= resistanceMultiplier;
            healthChange.value *= teamDamageMultiplier;
            
            health.current -= healthChange.value;
            
            if (health.current < 0)
            {
                healthChange.value += health.current;
                health.current = 0;
            }

            return healthChange;
        }
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;

            foreach (var e in stateChangedFilter.Value)
            {
                stateChangedFilter.Pools.Inc1.Del(e);
            }
            
            // pre process damages to multiply per team
            foreach (var e in damagePerTeamFilter.Value)
            {
                ref var health = ref damagePerTeamFilter.Pools.Inc1.Get(e);
                var damagePerTeam = damagePerTeamFilter.Pools.Inc2.Get(e);
                var player = damagePerTeamFilter.Pools.Inc3.Get(e);
                var alliedPlayers = player.GetAlliedPlayers();

                if (health.damages.Count == 0)
                {
                    continue;
                }
                
                for (var i = 0; i < health.damages.Count; i++)
                {
                    var damage = health.damages[i];
                    var damagePlayer = PlayerComponent.GetPlayerBitmask(damage.player);
                    
                    if ((alliedPlayers & damagePlayer) == damagePlayer)
                    {
                        // allied player
                        damage.value *= damagePerTeam.alliedDamageMultiplier;
                    }
                    else
                    {
                        damage.value *= damagePerTeam.enemyDamageMultiplier;
                    }

                    health.damages[i] = damage;
                }
            }

            foreach (var entity in filter.Value)
            {
                ref var health = ref filter.Pools.Inc1.Get(entity);
                
                health.temporaryInvulnerability.Decrease(deltaTime);
                health.processedDamages.Clear();
                health.processedHealEffects.Clear();
                
                health.timeSinceLastHit += deltaTime;

                health.previousAliveState = health.aliveType;
                
                var canReceiveDamage = health.temporaryInvulnerability.IsEmpty && !health.invulnerable;

                if (canReceiveDamage)
                {
                    for (var i = 0; i < health.damages.Count; i++)
                    {
                        var damage = health.damages[i];
                        health.timeSinceLastHit = 0;

                        if (DebugLogDamages)
                        {
                            Debug.Log($"DAMAGE VALUE ORIGINAL: [{entity}] {damage.value}");
                        }

                        var teamDamageMultiplier = DamageMultipliersPerTeam[damage.player];
                        damage = ProcessDamage(ref health, damage, teamDamageMultiplier);
                        
                        if (DebugLogDamages)
                        {
                            Debug.Log($"DAMAGE VALUE PROCESSED: [{entity}] {damage.value}");
                        }
                        
                        health.processedDamages.Add(damage);
                        health.temporaryInvulnerability.Fill();

                        if (health.temporaryInvulnerability.Total > 0)
                        {
                            break;
                        }
                    }
                }

                for (var i = 0; i < health.healEffects.Count; i++)
                {
                    var heal = health.healEffects[i];
                    health.current += heal.value;
                    if (health.current > health.total)
                    {
                        health.current = health.total;
                    }
                    
                    health.processedHealEffects.Add(heal);
                }
                
                health.damages.Clear();
                health.healEffects.Clear();

                if (health.current > 0 && health.triggerForceDeath)
                {
                    health.triggerForceDeath = false;
                    health.current = 0;
                }

                if (health.IsFull())
                {
                    health.timeInFullHealth += deltaTime;
                }
                else
                {
                    health.timeInFullHealth = 0;
                }

                if (health.previousAliveState != health.aliveType)
                {
                    world.AddComponent(entity, new HealthStateChangedEvent()
                    {
                        wasKilled = health.wasKilledLastFrame
                    });
                }
            }
        }
    }
}