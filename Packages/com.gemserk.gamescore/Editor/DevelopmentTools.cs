using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Directory = System.IO.Directory;

namespace Game.Editor
{
    public static class DevelopmentTools
    {
        [MenuItem("Tools/Create Folder for Current Scene")]
        public static void CreateFolderForCurrentScene()
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            if (activeScene == null)
            {
                return;
            }

            var sceneDirectory = Path.Combine(Path.GetDirectoryName(activeScene.path), 
                activeScene.name);

            if (!Directory.Exists(sceneDirectory))
            {
                // Debug.Log($"Creating {sceneDirectory}");
                Directory.CreateDirectory(sceneDirectory);
                AssetDatabase.Refresh();
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(sceneDirectory));
            }
            
            // AssetDatabase.GetAssetPath(activeScene.path);
            
        }
        
        [MenuItem("Tools/Open Project Temporary Folder")]
        public static void OpenTemporaryFolder()
        {
            EditorUtility.RevealInFinder(Application.temporaryCachePath);
        }
    }
}