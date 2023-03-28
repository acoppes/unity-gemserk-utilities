using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class UnityPhysics2dSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        [SerializeField]
        private bool disableSimulation;
        
        public void Init(EcsSystems systems)
        {
#if UNITY_2020_1_OR_NEWER
            Physics2D.simulationMode = SimulationMode2D.Script;
#else 
            Physics2D.autoSimulation = false;
#endif
        }
        
        public void Run(EcsSystems systems)
        {
            if (disableSimulation)
            {
                Physics2D.Simulate(Time.deltaTime);
            }
        }
    }
}