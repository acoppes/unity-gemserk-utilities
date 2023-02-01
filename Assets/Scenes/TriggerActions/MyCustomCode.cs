using UnityEngine;

namespace Scenes.TriggerActions
{
    public class MyCustomCode : MonoBehaviour
    {
        public void MyCustomMethod()
        {
            Debug.Log("Custom method called");   
        }

        public void MyCustomMethod2(int parameter1)
        {
            Debug.Log($"Custom method called {parameter1}");   
        }
    }
}
