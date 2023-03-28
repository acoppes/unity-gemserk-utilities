using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class UnityPhysicsSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        [SerializeField]
        private bool disableSimulation;
        
        public void Init(EcsSystems systems)
        {
            Physics.simulationMode = SimulationMode.Script;
        }
        
        public void Run(EcsSystems systems)
        {
            if (!disableSimulation)
            {
                Physics.Simulate(Time.deltaTime);
            }
        }
    }
}