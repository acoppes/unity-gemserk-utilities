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

        public List<string> GetSourceFiles()
        {
            var asepriteFiles = new List<string>();
            if (!string.IsNullOrEmpty(sourceFolder))
            {
                // TODO: get all folders inside sourceFolder
                if (!Path.IsPathRooted(sourceFolder))
                {
                    var absolutePath = Path.GetFullPath(sourceFolder, Application.dataPath);
                    asepriteFiles.AddRange(Directory.GetDirectories(absolutePath, "*", SearchOption.TopDirectoryOnly));
                    // asepriteFiles.AddRange(Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly));
                }
            }
            return asepriteFiles;
        }
    }
}