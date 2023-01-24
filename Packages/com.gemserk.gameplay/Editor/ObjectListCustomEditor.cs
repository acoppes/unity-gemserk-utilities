using UnityEditor;
using UnityEngine;

namespace Gemserk.Gameplay.Editor
{
    [CustomEditor(typeof(ObjectListAsset), true)]
    public class ObjectListCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var objectListAsset = target as ObjectListAsset;
            
            if (!Application.isPlaying)
            {
                if (GUILayout.Button("Reload"))
                {   
                    objectListAsset.Reload();
                }
            }
        }
    }
}