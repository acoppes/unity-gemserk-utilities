using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gemserk.Spine
{
    [CreateAssetMenu(menuName = "Gemserk/Spine Import Data")]
    public class SpineImportData : ScriptableObject
    {
        public const string GemserkSpineImporterSettings = "Gemserk/Spine Importer";
        
        private const string FileSearch = "*.spine";
        
        public string sourceFolder;
        public string outputFolder;

        public string sourceAbsolutePath => Path.GetFullPath(sourceFolder, Application.dataPath);
        public string outputAbsolutePath => Path.GetFullPath(outputFolder, Application.dataPath);

        // public bool recursive
        // public bool exportToFolders;
        
        #if UNITY_EDITOR
        [ContextMenu("Open Preferences")]
        public void OpenPreferences()
        {
            UnityEditor.SettingsService.OpenUserPreferences(GemserkSpineImporterSettings);
        }
        #endif
        
        public List<string> GetSourceFiles()
        {
            var asepriteFiles = new List<string>();
            if (!string.IsNullOrEmpty(sourceFolder))
            {
                if (!Path.IsPathRooted(sourceFolder))
                {
                    var absolutePath = Path.GetFullPath(sourceFolder, Application.dataPath);
                    asepriteFiles.AddRange(Directory.GetFiles(absolutePath, FileSearch, SearchOption.AllDirectories));
                }
            }
            return asepriteFiles;
        }
    }
}
