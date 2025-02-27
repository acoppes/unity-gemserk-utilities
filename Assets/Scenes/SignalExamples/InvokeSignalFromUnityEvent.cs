using UnityEngine;
using UnityEngine.Events;

public class InvokeSignalFromUnityEvent : MonoBehaviour
{
    public UnityEvent<GameObject> unityEvent;
    
    public void Start()
    {
        unityEvent.Invoke(gameObject);
    }
}