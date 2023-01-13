using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gemserk.Aseprite
{
    [CreateAssetMenu(menuName = "Gemserk/Aseprite Import Data")]
    public class AsepriteImportData : ScriptableObject
    {
        public const string GemserkAsepriteImporterSettings = "Gemserk/Aseprite Importer";
        
        private const string AsepriteFileSearch = "*.aseprite";
        
        public string sourceFolder;
        public string outputFolder;

        public string format;

        public string sourceAbsolutePath => Path.GetFullPath(sourceFolder, Application.dataPath);
        public string outputAbsolutePath => Path.GetFullPath(outputFolder, Application.dataPath);

        // public bool recursive
        // public bool exportToFolders;
        
        #if UNITY_EDITOR
        [ContextMenu("Open Preferences")]
        public void OpenPreferences()
        {
            UnityEditor.SettingsService.OpenUserPreferences(AsepriteImportData.GemserkAsepriteImporterSettings);
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
                    asepriteFiles.AddRange(Directory.GetFiles(absolutePath, AsepriteFileSearch, SearchOption.AllDirectories));
                }
            }
            return asepriteFiles;
        }
    }
}
