using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Spine.Editor
{
    public class SpineImportDataWindow : AssetListBaseWindow
    {
        public SpineImportDataWindow() : base(typeof(SpineImportData))
        {
            
        }
        
        [MenuItem("Window/Gemserk/SpineImportData Window")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<SpineImportDataWindow>();
            wnd.titleContent = new GUIContent("SpineImportData");
        }
    }
}