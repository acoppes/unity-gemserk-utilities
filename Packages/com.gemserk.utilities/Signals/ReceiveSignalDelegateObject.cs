using UnityEngine;
using UnityEngine.Events;

namespace Gemserk.Utilities.Signals
{
    public class ReceiveSignalDelegateObject : MonoBehaviour
    {
        public SignalAsset signalAsset;

        public UnityEvent<object> unityEvent;

        private void OnEnable()
        {
            signalAsset.Register(OnSignal);
        }

        private void OnDisable()
        {
            signalAsset.Unregister(OnSignal);
        }

        private void OnSignal(object userData)
        {
            unityEvent.Invoke(userData);
        }
    }
}