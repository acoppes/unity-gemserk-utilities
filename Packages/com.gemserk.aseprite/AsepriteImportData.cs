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

        private static string NormalizeRelativePath(string path)
        {
#if UNITY_EDITOR_WINDOWS
            return path.Replace('/', Path.DirectorySeparatorChar);
#else
            return path.Replace('\\', Path.DirectorySeparatorChar);
#endif
        }

        public string sourceAbsolutePath => Path.GetFullPath(NormalizeRelativePath(sourceFolder), Application.dataPath);
        public string outputAbsolutePath => Path.GetFullPath(NormalizeRelativePath(outputFolder), Application.dataPath);

        // public bool recursive
        // public bool exportToFolders;
        
        #if UNITY_EDITOR
        [ContextMenu("Open Preferences")]
        public void OpenPreferences()
        {
            UnityEditor.SettingsService.OpenUserPreferences(GemserkAsepriteImporterSettings);
        }
        #endif
        
        public List<string> GetSourceFiles()
        {
            var asepriteFiles = new List<string>();
            
            var normalizedFolder = NormalizeRelativePath(sourceFolder);
            
            if (!string.IsNullOrEmpty(normalizedFolder))
            {
                if (!Path.IsPathRooted(normalizedFolder))
                {
                    var absolutePath = Path.GetFullPath(normalizedFolder, Application.dataPath);
                    asepriteFiles.AddRange(Directory.GetFiles(absolutePath, AsepriteFileSearch, SearchOption.AllDirectories));
                }
            }
            return asepriteFiles;
        }
    }
}
