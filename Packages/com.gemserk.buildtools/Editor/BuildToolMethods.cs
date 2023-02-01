using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    [CustomEditor(typeof(BuildConfiguration), true)]
    public class BuildConfigurationCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button(new GUIContent("Load", null, "Load config in project settings")))
            {
                
            }

            if (GUILayout.Button(new GUIContent("Read current", null, "Overwrite with current project settings")))
            {
                // show dialog just in case
            }
        }
    }
}