using UnityEditor;
using UnityEngine;

namespace Gemserk.Spine.Editor
{
    public static class SpineImporterPreferences
    {
        private static bool prefsLoaded = false;

        private static string executablePath;
        
        [SettingsProvider]
        public static SettingsProvider CreateSelectionHistorySettingsProvider() {
            var provider = new SettingsProvider(SpineImportData.GemserkSpineImporterSettings, SettingsScope.User) {
                label = "Spine Importer",
                guiHandler = (searchContext) => {
                    
                    if (!prefsLoaded) 
                    {
                        executablePath = EditorPrefs.GetString(SpineImporter.GemserkSpineExecutablePath, "");
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
                            EditorPrefs.SetString(SpineImporter.GemserkSpineExecutablePath, executablePath);
                        }
                    }
                    EditorGUILayout.EndHorizontal();    
                    
                    if (GUI.changed) 
                    {
                        EditorPrefs.SetString(SpineImporter.GemserkSpineExecutablePath, executablePath);
                    }
                },

            };
            return provider;
        }
    }
}