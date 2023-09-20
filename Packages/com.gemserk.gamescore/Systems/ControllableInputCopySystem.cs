using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ControllableInputCopySystem : BaseSystem, IEcsRunSystem
    {
        // copy controls from unique input entity to each entity controlled by that control 
        readonly EcsFilterInject<Inc<InputComponent, PlayerControlComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<InputComponent, ControllableByComponent>, Exc<DisabledComponent>> controllables = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e0 in filter.Value)
            {
                var input0 = filter.Pools.Inc1.Get(e0);
                var playerControl = filter.Pools.Inc2.Get(e0);

                if (input0.actions == null)
                {
                    continue;
                }
                
                var keys = input0.actions.Keys;
                
                foreach (var e1 in controllables.Value)
                {
                    ref var controllable = ref controllables.Pools.Inc2.Get(e1);
                    
                    if (!controllable.IsControllableByPlayer())
                        continue;
                    
                    if (controllable.playerControlId != playerControl.controlId)
                        continue;
                    
                    ref var input1 = ref controllables.Pools.Inc1.Get(e1);
                    
                    foreach (var inputName in keys)
                    {
                        input1.actions[inputName].Copy(input0.actions[inputName]);
                    }
                }
            }
        }
    }
}