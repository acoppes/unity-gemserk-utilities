using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Gameplay.Signals
{
    [CreateAssetMenu(menuName = "Gemserk/Gameplay/Signal")]
    public class SignalAsset : ScriptableObject, ISignal
    {
        private readonly IList<ISignal.OnSignal> handlers = new List<ISignal.OnSignal>();

        public void Register(ISignal.OnSignal signalHandler)
        {
            handlers.Add(signalHandler);
        }

        public void Unregister(ISignal.OnSignal signalHandler)
        {
            handlers.Remove(signalHandler);
        }

        public void Signal(object userData)
        {
            foreach (var onSignal in handlers)
            {
                onSignal(userData);
            }
        }
    }
}
