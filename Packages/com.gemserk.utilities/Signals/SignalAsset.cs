using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities.Signals
{
    [CreateAssetMenu(menuName = "Gemserk/Gameplay/Signal")]
    public class SignalAsset : ScriptableObject, ISignal
    {
        private readonly IList<ISignal.OnSignal> handlers = new List<ISignal.OnSignal>();
        private readonly IList<ISignal.OnSignalMultiple> handlersMultipleParams = new List<ISignal.OnSignalMultiple>();

        public IList<ISignal.OnSignal> GetHandlers() => handlers;
        public IList<ISignal.OnSignalMultiple> GetHandlersMultiples() => handlersMultipleParams;

        public void Register(ISignal.OnSignal signalHandler)
        {
            handlers.Add(signalHandler);
        }

        public void Unregister(ISignal.OnSignal signalHandler)
        {
            handlers.Remove(signalHandler);
        }

        public void Register(ISignal.OnSignalMultiple signalHandler)
        {
            handlersMultipleParams.Add(signalHandler);
        }

        public void Unregister(ISignal.OnSignalMultiple signalHandler)
        {
            handlersMultipleParams.Remove(signalHandler);
        }

        public void Signal(object userData)
        {
            foreach (var onSignal in handlers)
            {
                onSignal(userData);
            }
        }

        public void Signal(params object[] userData)
        {
            foreach (var onSignalMultiple in handlersMultipleParams)
            {
                onSignalMultiple(userData);
            }
        }

        public void Clear()
        {
            handlers.Clear();
            handlersMultipleParams.Clear();
        }
    }
}
