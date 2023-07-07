using System;
using Gemserk.Utilities.Signals;
using UnityEngine;

public class SignalExampleByCode : MonoBehaviour
{
    public SignalAsset signal;

    private void OnEnable()
    {
        signal.Register(OnSignalInvoked);
    }

    private void OnDisable()
    {
        signal.Unregister(OnSignalInvoked);
    }

    // Start is called before the first frame update
    void Start()
    {
        signal.Signal(this);       
    }

    private void OnSignalInvoked(object userdata)
    {
        Debug.Log($"Signal Invoked: {userdata}");
    }
}
