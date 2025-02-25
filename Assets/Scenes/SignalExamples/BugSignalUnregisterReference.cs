using Gemserk.Utilities.Signals;
using UnityEngine;

public class BugSignalUnregisterReference : MonoBehaviour
{
    public SignalAsset signal;

    private void OnEnable()
    {
        signal.Register(OnSignalInvoked);
    }

    // this class forgot to unregister
    // private void OnDisable()
    // {
    //     signal.Unregister(OnSignalInvoked);
    // }
    
    private void OnSignalInvoked(object userdata)
    {
        Debug.Log($"Signal Invoked: {userdata}");
    }
}