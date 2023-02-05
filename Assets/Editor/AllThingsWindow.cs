using Gemserk.Aseprite;
using Gemserk.Aseprite.Editor;
using Gemserk.BuildTools;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class AllThingsWindow : AssetListBaseWindow
{
    public AllThingsWindow() : base(typeof(AsepriteImportData), typeof(BuildConfiguration))
    {
            
    }
        
    [MenuItem("Window/Gemserk/All Things Window")]
    public static void ShowWindow()
    {
        EditorWindow wnd = GetWindow<AllThingsWindow>();
        wnd.titleContent = new GUIContent("Assets List Window");
    }
}