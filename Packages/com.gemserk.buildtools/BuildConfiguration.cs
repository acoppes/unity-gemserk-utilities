using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.BuildTools
{
    [CreateAssetMenu(menuName = "Gemserk/Build Configuration")]
    public class BuildConfiguration : ScriptableObject
    {
        [FolderPath()]
        public string settingsFolder;

// #if UNITY_EDITOR
//         // public string[] scenes;
//         public List<UnityEditor.SceneAsset> sceneAssets = new List<SceneAsset>();
// #endif
    }
}
