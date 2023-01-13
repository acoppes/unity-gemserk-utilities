using UnityEditor;
using UnityEngine;

namespace Gemserk.Aseprite.Editor
{
    public static class AsepriteImporterPreferences
    {
        private static bool prefsLoaded = false;

        private static string asepritePath;
        
        [SettingsProvider]
        public static SettingsProvider CreateSelectionHistorySettingsProvider() {
            var provider = new SettingsProvider(AsepriteImportData.GemserkAsepriteImporterSettings, SettingsScope.User) {
                label = "Aseprite Importer",
                guiHandler = (searchContext) => {
                    
                    if (!prefsLoaded) 
                    {
                        asepritePath = EditorPrefs.GetString(AsepriteImporter.GemserkAsepriteExecutablePath, "");
                        prefsLoaded = true;
                    }

                    EditorGUILayout.BeginHorizontal();
                    asepritePath = EditorGUILayout.TextField("Executable Path", asepritePath);
                    if (GUILayout.Button("Browse"))
                    {
                        var executablePath = EditorUtility.OpenFilePanel("Executable Path", "", "exe");
                
                        if (!string.IsNullOrEmpty(executablePath))
                        {
                            asepritePath = executablePath;
                            EditorPrefs.SetString(AsepriteImporter.GemserkAsepriteExecutablePath, asepritePath);
                        }
                    }
                    EditorGUILayout.EndHorizontal();    
                    
                    if (GUI.changed) 
                    {
                        EditorPrefs.SetString(AsepriteImporter.GemserkAsepriteExecutablePath, asepritePath);
                    }
                },

            };
            return provider;
        }
    }
}