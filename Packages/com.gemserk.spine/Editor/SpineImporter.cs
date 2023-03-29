using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Gemserk.Spine.Editor
{
    public static class SpineImporter
    {
        public const string GemserkSpineExecutablePath = "Gemserk.Spine.ExecutablePath";
        
        // http://en.esotericsoftware.com/spine-command-line-interface
        // Spine -i SpineBoy.spine -o exported/SpineBoy
        
        [MenuItem("Assets/Spine/Reimport All")]
        [Shortcut("Gemserk/Spine/Reimport All", KeyCode.A, ShortcutModifiers.Shift | ShortcutModifiers.Alt)]
        public static void ReimportAll()
        {
            var executablePath = EditorPrefs.GetString(GemserkSpineExecutablePath);

            if (string.IsNullOrEmpty(executablePath))
            {
                if (!EditorUtility.DisplayDialog("Not configured", "Executable path is not configured, configure it now?", "Configure", "Cancel"))
                {
                    return;
                }
                
                SettingsService.OpenUserPreferences(SpineImportData.GemserkSpineImporterSettings);
                return;
            }
            
            var guids = AssetDatabase.FindAssets($"t:{nameof(SpineImportData)}");
            var importDataAssets = guids.Select(g => AssetDatabase.LoadAssetAtPath<SpineImportData>(
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
                        
                        ImportFile(executablePath, file, importData.outputAbsolutePath);
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
        
        public static void ImportFile(string executablePath, string filePath, string outputFolder)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // var fileOutputFolderPath = Path.Combine(outputFolder, fileName);
            var targetFolder = Path.Combine(outputFolder, fileName);
          
            // var fileOutputFolderPath = Path.GetFullPath(Path.Combine("SpineImporter", fileName), Application.temporaryCachePath);
            //
            // if (!Directory.Exists(fileOutputFolderPath))
            // {
            //     Directory.CreateDirectory(fileOutputFolderPath);
            // }
            
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            
            // var outputFormat = format.Replace("{title}", fileName);
            
            ExecuteSpineExporter(executablePath, filePath, targetFolder);

            // var generatedFiles = Directory.GetFiles(fileOutputFolderPath);
            
            // var targetDirectory = new DirectoryInfo(targetFolder);
            //
            // Debug.Log($"Cleaning up pngs from {targetFolder}");
            // foreach (var file in targetDirectory.EnumerateFiles("*.png")) {
            //     // Debug.Log(file);
            //     file.Delete();
            // }
            //
            // Debug.Log($"Copy from temporary folder {outputFolder} to {targetFolder}");
            // foreach (var source in generatedFiles)
            // {
            //     var destination = Path.Combine(targetFolder, Path.GetFileName(source));
            //     FileUtil.CopyFileOrDirectory(source, destination);
            // }
        }

        public static void ExecuteSpineExporter(string executablePath, string inputPath, string outputPath)
        {
            var process = new Process();

            var arguments = $"-i {inputPath} -o \"{outputPath}\"";
            
            process.StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Maximized
            };
            
            process.Start();
            process.WaitForExit();
        }
    }
}