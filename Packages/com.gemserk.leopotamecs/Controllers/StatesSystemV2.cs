using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystemV2 : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<StatesComponentV2>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponentV2> stateComponents = default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStatesComponent(ref StatesComponentV2 states, float dt)
        {
            for (var i = 0; i < states.states.Length; i++)
            {
                if (states.HasState(i))
                {
                    ref var state = ref states.GetState(i);
                    state.time += dt;
                    state.updateCount++;
                    
                    if (state.duration > 0 && state.time > state.duration)
                    {
                        states.Exit(i);
                    }
                }
            }
        }

        public void Run(EcsSystems systems)
        {
            var dt = this.dt;
            
            foreach (var entity in statesFilter.Value)
            {
                ref var states = ref stateComponents.Value.Get(entity);
                UpdateStatesComponent(ref states, dt);
            }
        }
    }
}