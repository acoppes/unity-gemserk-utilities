using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using UnityEngine;

namespace Game.Components
{
    public static class ComponentExtensions
    {
        public static bool IsStartFrame(this State state)
        {
            return state.updateCount == 0;
        }
        
        public static bool HasControllableType(this ControllableByComponent.ControllableType self, ControllableByComponent.ControllableType flag)
        {
            return (self & flag) == flag;
        }
        
        public static bool HasColliderType(this PhysicsComponent.ColliderType self, PhysicsComponent.ColliderType flag)
        {
            return (self & flag) == flag;
        }
        
        public static bool HasFollowType(this FollowEntityComponent.FollowType self, FollowEntityComponent.FollowType flag)
        {
            return (self & flag) == flag;
        }

        public static void ExitStatesAndSubStates(this StatesComponent statesComponent, string stateName)
        {
            var statesToExit = statesComponent.activeStates
                .Where(s => s.StartsWith(stateName, StringComparison.OrdinalIgnoreCase)).ToList();
            
            foreach (var stateToExit in statesToExit)
            {
                statesComponent.ExitState(stateToExit);
            }
        }

        public static Vector3 ApplyDirection(this LookingDirection lookingDirection, Vector3 v)
        {
            v.x *= lookingDirection.value.x > 0 ? 1 : -1;
            return v;
        }
        
        public static void EnterStates(this StatesComponent statesComponent, IEnumerable<string> states, float duration = 0)
        {
            foreach (var state in states)
            {
                statesComponent.EnterState(state, duration);
            }
        }
        
        public static void ExitStates(this StatesComponent statesComponent, IEnumerable<string> states, float duration = 0)
        {
            foreach (var state in states)
            {
                statesComponent.ExitState(state, duration);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAlliedPlayers(this PlayerComponent playerComponent)
        {
            return PlayerAllianceExtensions.GetAlliedPlayers(playerComponent.player);
        }
    }
}