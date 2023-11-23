using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Game.Editor.Tools
{
    [InitializeOnLoad]
    public static class MaximizeOnPlayIfCustomPlayButton
    {
        private const string GemserkEditorStartGameMaximized = "Gemserk.Editor.StartGameMaximized";
        private const string GemserkToggleStartGameMaximizedMenuItem = "Gemserk/Maximize Game Window on Play From Scene";

        // register an event handler when the class is initialized
        static MaximizeOnPlayIfCustomPlayButton()
        {
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode && EditorSceneManager.playModeStartScene != null)
            {
                var overrideStartMaximized = EditorPrefs.GetBool(GemserkEditorStartGameMaximized, true);
                
                if (UnityEditor.EditorWindow.focusedWindow != null && overrideStartMaximized)
                {
                    // Debug.Log(UnityEditor.EditorWindow.focusedWindow.GetType().Name);
                    EditorCoroutineUtility.StartCoroutine(SetMaximizeWithDelay(), UnityEditor.EditorWindow.focusedWindow);
                }
            }
        }

        private static IEnumerator SetMaximizeWithDelay()
        {
            yield return null;
            UnityEditor.EditorWindow.focusedWindow.maximized = true;
        }
        
        [MenuItem(GemserkToggleStartGameMaximizedMenuItem)]
        public static void ToggleStartGameMaximized()
        {
            var maximized = EditorPrefs.GetBool(GemserkEditorStartGameMaximized, true);
            EditorPrefs.SetBool(GemserkEditorStartGameMaximized, !maximized);
        }
        
        
        [MenuItem(GemserkToggleStartGameMaximizedMenuItem, true)]
        public static bool ToggleStartGameMaximizedValidator(MenuCommand menuCommand)
        {
            var isChecked = EditorPrefs.GetBool(GemserkEditorStartGameMaximized, true);
            Menu.SetChecked(GemserkToggleStartGameMaximizedMenuItem, isChecked);
            return true;
        }
    }
}