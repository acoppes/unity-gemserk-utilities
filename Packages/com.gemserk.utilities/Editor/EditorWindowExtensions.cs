using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public static class EditorWindowExtensions
    {
        public static void OpenScriptToEdit(string scriptName)
        {
            var files = AssetDatabase.FindAssets($"t:Script {scriptName}");
                
            if (files.Length >0)
            {
                var path = AssetDatabase.GUIDToAssetPath(files[0]);
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
            }
        }
        
        public static void AddEditScript(GenericMenu genericMenu, string scriptName)
        {
            genericMenu.AddItem(new GUIContent("Edit Script", "Opens the script in the IDE."), false, delegate
            {
                OpenScriptToEdit(scriptName);
            });
        }
    }
}