using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [Serializable]
    public class Comments
    {
        [TextArea(2,5)]
        public string value;
    }
    
    public class EditorComment : MonoBehaviour
    {
        #if UNITY_EDITOR
        public Comments comments;
        #endif
    }
}