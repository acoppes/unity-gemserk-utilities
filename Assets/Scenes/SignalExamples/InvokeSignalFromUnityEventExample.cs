using UnityEngine;
using UnityEngine.Events;

public class InvokeSignalFromUnityEventExample : MonoBehaviour
{
    public UnityEvent<GameObject> unityEvent;
    
    public void Start()
    {
        unityEvent.Invoke(gameObject);
    }
}