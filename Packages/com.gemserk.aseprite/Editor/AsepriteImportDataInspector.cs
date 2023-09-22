using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Aseprite.Editor
{
    [CustomEditor(typeof(AsepriteImportData))]
    public class AsepriteImportDataInspector : UnityEditor.Editor
    {
        private List<string> sourceFiles = new ();

        private Vector2 scroll;
        
        private void OnEnable()
        {
            ReloadFiles();
        }

        private void ReloadFiles()
        {
            if (target != null)
            {
                var importData = target as AsepriteImportData;
                sourceFiles = importData.GetSourceFiles();
            }
        }

        public override void OnInspectorGUI()
        {
            var asepriteExecutablePath = EditorPrefs.GetString(AsepriteImporter.GemserkAsepriteExecutablePath);

            var asepriteConfigured = !string.IsNullOrEmpty(asepriteExecutablePath);
            
            if (!asepriteConfigured)
            {
                EditorGUILayout.LabelField("Aseprite executable is not configured.");
                if (GUILayout.Button("Configure"))
                {
                    SettingsService.OpenUserPreferences(AsepriteImportData.GemserkAsepriteImporterSettings);
                }
                EditorGUILayout.Separator();
            }
            
            var importData = target as AsepriteImportData;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Source Folder", GUILayout.Width(90));
            importData.sourceFolder = EditorGUILayout.TextField(importData.sourceFolder, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                var absolutePath = Path.GetFullPath(importData.sourceFolder, Application.dataPath);
                var newFolder = EditorUtility.OpenFolderPanel("Source Folder", absolutePath, "");
                if (!string.IsNullOrEmpty(newFolder))
                {
                    importData.sourceFolder = Path.GetRelativePath(Application.dataPath, newFolder);
                    ReloadFiles();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Output Folder", GUILayout.Width(90));
            importData.outputFolder = EditorGUILayout.TextField(importData.outputFolder, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                var absolutePath = Path.GetFullPath(importData.outputFolder, Application.dataPath);
                var newFolder = EditorUtility.OpenFolderPanel("Output Folder", absolutePath, "");

                if (!string.IsNullOrEmpty(newFolder))
                {
                    importData.outputFolder = Path.GetRelativePath(Application.dataPath, newFolder);
                }
            }
            EditorGUILayout.EndHorizontal();

            
            importData.format = EditorGUILayout.TextField("Format", importData.format);

            EditorGUILayout.Separator();
            
            EditorGUI.BeginDisabledGroup(!asepriteConfigured);
            if (GUILayout.Button("Reimport All", GUILayout.ExpandWidth(true)))
            {
                foreach (var file in sourceFiles)
                {
                    AsepriteImporter.ImportFile(asepriteExecutablePath, file, 
                        importData.outputAbsolutePath, importData.format);
                }
                
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var file in sourceFiles)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(Path.GetFileName(file));
                
                EditorGUI.BeginDisabledGroup(!asepriteConfigured);
                if (GUILayout.Button("Import", GUILayout.ExpandWidth(true)))
                {
                    AsepriteImporter.ImportFile(asepriteExecutablePath, file, 
                        importData.outputAbsolutePath, importData.format);
                    AssetDatabase.Refresh();
                }
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(importData);
            }
            
            if (GUILayout.Button("Reload List", GUILayout.ExpandWidth(true)))
            {
                ReloadFiles();
                Repaint();
            }
        }
    }
}