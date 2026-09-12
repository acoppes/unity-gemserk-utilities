using System.Collections.Generic;
using System.IO;
using Game.Components;
using UnityEngine;

namespace Game.Definitions
{
    /// <summary>
    /// Creates a list of AnimationAssets from a list of PNG files.
    /// </summary>
    [CreateAssetMenu(menuName = "Gemserk/Animations Import Data")]
    public class AnimationsImportData : ScriptableObject
    {
        public float defaultFps = AnimationDefinition.DefaultFrameRate;
        
        public string sourceFolder;
        public string outputFolder;
        
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

        public List<string> GetSourceFiles()
        {
            var asepriteFiles = new List<string>();
            if (!string.IsNullOrEmpty(sourceFolder))
            {
                // TODO: get all folders inside sourceFolder
                if (!Path.IsPathRooted(sourceFolder))
                {
                    asepriteFiles.AddRange(Directory.GetDirectories(sourceAbsolutePath, "*", SearchOption.TopDirectoryOnly));
                    // asepriteFiles.AddRange(Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly));
                }
            }
            return asepriteFiles;
        }
    }
}