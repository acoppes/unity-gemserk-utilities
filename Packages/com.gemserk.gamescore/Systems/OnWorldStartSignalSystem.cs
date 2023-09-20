using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class OnWorldStartSignalSystem : BaseSystem, IEcsRunSystem
    {
        public SignalAsset onWorldStartSignal;

        private bool signal;
        
        public void Run(EcsSystems systems)
        {
            if (!signal && onWorldStartSignal != null)
            {
                onWorldStartSignal.Signal(null);
                signal = true;
            }
        }
    }
}