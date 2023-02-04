using System.Collections.Generic;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public class AssetListBaseWindow<T> : EditorWindow where T : Object
    {
        protected List<T> objectsList = new ();
        protected bool[] foldouts = new bool[100];

        private Vector2 position;

        private void OnFocus()
        {
            objectsList = AssetDatabaseExt.FindAssets<T>();
            foldouts = new bool[objectsList.Count];
        }

        private void OnGUI()
        {
            position = EditorGUILayout.BeginScrollView(position, false, false);
            EditorGUILayout.BeginVertical();
            for (var i = 0; i < objectsList.Count; i++)
            {
                var asset = objectsList[i];
                if (asset == null)
                {
                    continue;
                }
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], asset.name);
                if (foldouts[i])
                {
                    EditorGUI.indentLevel++;
                    var objectEditor = UnityEditor.Editor.CreateEditor(asset);
                    objectEditor.OnInspectorGUI();
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}