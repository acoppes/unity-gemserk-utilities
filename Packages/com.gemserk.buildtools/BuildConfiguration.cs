using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.BuildTools
{
    [CreateAssetMenu(menuName = "Gemserk/Build Configuration")]
    public class BuildConfiguration : ScriptableObject
    {
        // [FolderPath()]
        // public string settingsFolder;

        public string productName;
        public string version = "0.0.1";
        public int defaultWebScreenWidth;
        public int defaultWebScreenHeight;

#if UNITY_EDITOR
        // public string[] scenes;
        public List<UnityEditor.SceneAsset> sceneAssets = new List<UnityEditor.SceneAsset>();
#endif
    }
}
