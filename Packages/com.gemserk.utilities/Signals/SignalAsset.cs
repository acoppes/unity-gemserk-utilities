using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities.Signals
{
    [CreateAssetMenu(menuName = "Gemserk/Gameplay/Signal")]
    public class SignalAsset : ScriptableObject, ISignal
    {
        private readonly IList<ISignal.OnSignal> handlers = new List<ISignal.OnSignal>();

        public IList<ISignal.OnSignal> GetHandlers() => handlers;

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

        public void Clear()
        {
            handlers.Clear();
        }
    }
}
