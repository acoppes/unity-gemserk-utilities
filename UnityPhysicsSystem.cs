using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class UnityPhysicsSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            Physics.simulationMode = SimulationMode.Script;
        }
        
        public void Run(EcsSystems systems)
        {
            Physics.Simulate(Time.deltaTime);
        }
    }
}