using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Aseprite.Editor
{
    public class AsepriteImportDataWindow : AssetListBaseWindow, IHasCustomMenu
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
        
        public void AddItemsToMenu(GenericMenu menu)
        {
            EditorWindowExtensions.AddEditScript(menu, nameof(AsepriteImportDataWindow));
        }
    }
}