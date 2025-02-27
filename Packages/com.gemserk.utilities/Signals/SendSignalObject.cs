using UnityEngine;

namespace Gemserk.Utilities.Signals
{
    // this class is used to connect buttons, unity events or animator to send a signal
    public class SendSignalObject : MonoBehaviour
    {
        public SignalAsset signalAsset;

        public void SendSignal()
        {
            signalAsset.Signal(null);
        }
        
        public void SendSignalWithGameObject(GameObject source)
        {
            signalAsset.Signal(source);
        }

        public void SendSignalWithObject(object source)
        {
            signalAsset.Signal(source);
        }
    }
}