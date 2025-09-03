using UnityEngine;

namespace Gemserk.Utilities
{
    public class OnAwakeUnparent : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
        }
    }
}