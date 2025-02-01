using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomEditor(typeof(ObjectListAsset), true)]
    public class ObjectListCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var objectListAsset = target as ObjectListAsset;
            
            if (!objectListAsset)
                return;
            
            if (!Application.isPlaying)
            {
                // if (GUILayout.Button("Browse path"))
                // {
                //     var absolutePath = Path.GetFullPath(objectListAsset.objectList.Path, 
                //         Application.dataPath);
                //     
                //     var newFolder = EditorUtility.OpenFolderPanel("Path", absolutePath, "");
                //     if (!string.IsNullOrEmpty(newFolder))
                //     {
                //         objectListAsset.objectList.Path = Path.GetRelativePath(Application.dataPath, newFolder);
                //         objectListAsset.objectList.Reload();
                //         EditorUtility.SetDirty(objectListAsset);
                //     }
                // }
                
                
                if (objectListAsset.objectList != null && objectListAsset.objectList.normalizedAssetPaths.Length > 0)
                {
                    if (GUILayout.Button("Reload from paths"))
                    {
                        objectListAsset.objectList?.Reload();
                        EditorUtility.SetDirty(objectListAsset);
                    }
                }
            }
        }
    }
    
    [CustomEditor(typeof(ObjectListMonoBehaviour), true)]
    public class ObjectListMonoBehaviourCustomEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var objectListAsset = target as ObjectListMonoBehaviour;
            
            if (objectListAsset != null && objectListAsset.objectList != null)
            {
                objectListAsset.objectList?.Reload();
                EditorUtility.SetDirty(objectListAsset);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            
            var defaultInspectorChanged = EditorGUI.EndChangeCheck();

            var objectListAsset = target as ObjectListMonoBehaviour;
            
            if (!Application.isPlaying)
            {
                // if (GUILayout.Button("Browse path"))
                // {
                //     var newFolder = GUIInternalUtils.SelectLocalFolder(objectListAsset.objectList.Path);
                //     
                //     if (!string.IsNullOrEmpty(newFolder))
                //     {
                //         objectListAsset.objectList.Path = newFolder;
                //         objectListAsset.objectList.Reload();
                //         EditorUtility.SetDirty(objectListAsset);
                //     }
                // }
                
                if (GUILayout.Button("Reload") || defaultInspectorChanged)
                {
                    objectListAsset.objectList?.Reload();
                    EditorUtility.SetDirty(objectListAsset);
                }
            }
        }
    }
}