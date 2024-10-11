using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [Serializable]
    public class EditorNote
    {
        [TextArea(2,5)]
        public string note;
    }
    
    public class EditorNoteMonoBehaviour : MonoBehaviour
    {
        public EditorNote note;
    }
    
    
}