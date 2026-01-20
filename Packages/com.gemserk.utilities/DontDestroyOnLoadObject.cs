using UnityEngine;

namespace Gemserk.Utilities
{
    public class DontDestroyOnLoadObject : MonoBehaviour
    {
        public bool autoMoveToRoot;
        
        private void Awake()
        {
            if (autoMoveToRoot)
            {
                gameObject.transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}