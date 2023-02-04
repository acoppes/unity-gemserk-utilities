using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    public class BuildConfigurationsWindow : AssetListBaseWindow<BuildConfiguration>
    {
        [MenuItem("Window/Gemserk/Build Configurations")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<BuildConfigurationsWindow>();
            wnd.titleContent = new GUIContent("Build Configurations");
        }
    }
}