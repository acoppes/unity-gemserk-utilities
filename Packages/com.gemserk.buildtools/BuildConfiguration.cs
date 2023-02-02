using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools
{
    [CreateAssetMenu(menuName = "Gemserk/Build Configuration")]
    public class BuildConfiguration : ScriptableObject
    {
        [SelectFile(true)]
        public string settingsFolder;

#if UNITY_EDITOR
        // public string[] scenes;
        public List<UnityEditor.SceneAsset> sceneAssets = new List<SceneAsset>();
#endif
    }
}
