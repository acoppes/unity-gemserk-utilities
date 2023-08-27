using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyGame;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystemV2 : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<StatesComponentV2>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponentV2> stateComponents = default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStateTime(ref StatesComponentV2 statesComponent, float dt)
        {
            for (var i = 0; i < statesComponent.states.Length; i++)
            {
                if (statesComponent.HasState(i))
                {
                    var state = statesComponent.states[i];
                    state.time += dt;
                    statesComponent.states[i] = state;

                    if (state.duration > 0 && state.time > state.duration)
                    {
                        statesComponent.Exit(i);
                    }
                }
            }
        }

        public void Run(EcsSystems systems)
        {
            var dt = this.dt;
            
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);
                UpdateStateTime(ref statesComponent, dt);
            }
        }
    }
}