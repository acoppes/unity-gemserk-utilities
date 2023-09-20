using Game.Definitions;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class AnimationsAssetWindow : AssetListBaseWindow
    {
        public AnimationsAssetWindow() : base(typeof(AnimationsAsset))
        {
            
        }
        
        [MenuItem("Window/Gemserk/Animations")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<AnimationsAssetWindow>();
            wnd.titleContent = new GUIContent("Animations List");
        }
    }
}