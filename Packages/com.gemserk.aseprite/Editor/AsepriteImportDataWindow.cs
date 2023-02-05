using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Aseprite.Editor
{
    public class AsepriteImportDataWindow : AssetListBaseWindow
    {
        public AsepriteImportDataWindow() : base(typeof(AsepriteImportData))
        {
            
        }
        
        [MenuItem("Window/Gemserk/AsepriteImportData Window")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<AsepriteImportDataWindow>();
            wnd.titleContent = new GUIContent("AsepriteImportDatas");
        }
    }
}