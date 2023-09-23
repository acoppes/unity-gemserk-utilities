using UnityEngine;

namespace Scenes.TriggerActions
{
    public class MyCustomCode : MonoBehaviour
    {
        public void MyCustomMethod()
        {
            Debug.Log("Custom method1 called");   
        }

        public void MyCustomMethod2(int parameter1)
        {
            Debug.Log($"Custom method2 called {parameter1}");   
        }
        
        public void MyCustomMethod3(object activator)
        {
            Debug.Log($"Custom method3 called {activator}");   
        }
    }
}
