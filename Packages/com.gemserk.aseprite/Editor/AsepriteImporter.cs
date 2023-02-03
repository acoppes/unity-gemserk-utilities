using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Gemserk.Aseprite.Editor
{
    public static class AsepriteImporter
    {
        public const string GemserkAsepriteExecutablePath = "Gemserk.Aseprite.ExecutablePath";
        
        // Aseprite.exe -b NinjaCat.aseprite --save-as exported/NinjaCot-{tag}-{tagframe0000}.png
        // https://www.aseprite.org/docs/cli/#use-cases
        
        [MenuItem("Assets/Aseprite/Reimport All")]
        [Shortcut("Gemserk/Reimport All", KeyCode.A, ShortcutModifiers.Shift | ShortcutModifiers.Alt)]
        public static void ReimportAll()
        {
            var asepriteExecutablePath = EditorPrefs.GetString(GemserkAsepriteExecutablePath);

            if (string.IsNullOrEmpty(asepriteExecutablePath))
            {
                if (!EditorUtility.DisplayDialog("Not configured", "Aseprite executable path is not configured, configure it now?", "Configure", "Cancel"))
                {
                    return;
                }
                
                SettingsService.OpenUserPreferences(AsepriteImportData.GemserkAsepriteImporterSettings);
                return;
            }
            
            var guids = AssetDatabase.FindAssets($"t:{nameof(AsepriteImportData)}");
            var importDataAssets = guids.Select(g => AssetDatabase.LoadAssetAtPath<AsepriteImportData>(
                AssetDatabase.GUIDToAssetPath(g))).ToList();

            try
            {
                foreach (var importData in importDataAssets)
                {
                    if (EditorUtility.DisplayCancelableProgressBar($"Importing from {AssetDatabase.GetAssetPath(importData)}", "Starting...",
                            0))
                    {
                        return;
                    }
                    
                    var sourceFiles = importData.GetSourceFiles();
                    var progress = 0.0f;
                    var increment = 1.0f / sourceFiles.Count;

                    progress += increment;
                    
                    foreach (var file in sourceFiles)
                    {
                        if (EditorUtility.DisplayCancelableProgressBar($"Importing from {AssetDatabase.GetAssetPath(importData)}", $"{Path.GetFileName(file)}",
                                progress))
                        {
                            return;
                        }
                        
                        ImportFile(asepriteExecutablePath, file, importData.outputAbsolutePath, importData.format);
                        progress += increment;
                    }
                }
                
                AssetDatabase.Refresh();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        
        public static void ImportFile(string asepritePath, string filePath, string outputFolder, string format)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // var fileOutputFolderPath = Path.Combine(outputFolder, fileName);
            var targetFolder = Path.Combine(outputFolder, fileName);
            var fileOutputFolderPath = Path.GetFullPath(Path.Combine("AsepriteImporter", fileName), Application.temporaryCachePath);

            if (!Directory.Exists(fileOutputFolderPath))
            {
                Directory.CreateDirectory(fileOutputFolderPath);
            }
            
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            
            var outputFormat = format.Replace("{title}", fileName);
            
            ExecuteAsepriteExporter(asepritePath, filePath, fileOutputFolderPath, outputFormat);

            var generatedFiles = Directory.GetFiles(fileOutputFolderPath);
            
            var targetDirectory = new DirectoryInfo(targetFolder);
            
            Debug.Log($"Cleaning up pngs from {targetFolder}");
            foreach (var file in targetDirectory.EnumerateFiles("*.png")) {
                // Debug.Log(file);
                file.Delete();
            }
            
            Debug.Log($"Copy from temporary folder {outputFolder} to {targetFolder}");
            foreach (var source in generatedFiles)
            {
                var destination = Path.Combine(targetFolder, Path.GetFileName(source));
                FileUtil.CopyFileOrDirectory(source, destination);
            }
        }

        public static void ExecuteAsepriteExporter(string asepritePath, string filePath, string fileOutputFolderPath, string outputFormat)
        {
            var process = new Process();

            var arguments = $"-b {filePath} --save-as \"{fileOutputFolderPath}/{outputFormat}\"";
            
            process.StartInfo = new ProcessStartInfo
            {
                FileName = asepritePath,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Maximized
            };
            
            process.Start();
            process.WaitForExit();
        }
    }
}