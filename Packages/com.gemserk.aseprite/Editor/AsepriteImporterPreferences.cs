using UnityEditor;
using UnityEngine;

namespace Gemserk.Aseprite.Editor
{
    public static class AsepriteImporterPreferences
    {
        private static bool prefsLoaded = false;

        private static string executablePath;
        
        [SettingsProvider]
        public static SettingsProvider CreateSelectionHistorySettingsProvider() {
            var provider = new SettingsProvider(AsepriteImportData.GemserkAsepriteImporterSettings, SettingsScope.User) {
                label = "Aseprite Importer",
                guiHandler = (searchContext) => {
                    
                    if (!prefsLoaded) 
                    {
                        executablePath = EditorPrefs.GetString(AsepriteImporter.GemserkAsepriteExecutablePath, "");
                        prefsLoaded = true;
                    }

                    EditorGUILayout.BeginHorizontal();
                    executablePath = EditorGUILayout.TextField("Executable Path", executablePath);
                    if (GUILayout.Button("Browse"))
                    {
                        var newExecutablePath = EditorUtility.OpenFilePanel("Executable Path", "", "exe");
                
                        if (!string.IsNullOrEmpty(newExecutablePath))
                        {
                            executablePath = newExecutablePath;
                            EditorPrefs.SetString(AsepriteImporter.GemserkAsepriteExecutablePath, executablePath);
                        }
                    }
                    EditorGUILayout.EndHorizontal();    
                    
                    if (GUI.changed) 
                    {
                        EditorPrefs.SetString(AsepriteImporter.GemserkAsepriteExecutablePath, executablePath);
                    }
                },

            };
            return provider;
        }
    }
}