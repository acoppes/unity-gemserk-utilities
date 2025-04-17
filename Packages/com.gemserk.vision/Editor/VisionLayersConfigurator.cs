using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Vision.Editor
{
    [InitializeOnLoad]
    static class VisionLayersConfigurator
    {
        static VisionLayersConfigurator()
        {
            // create layers for vision system if they dont exist!
            ConfigureVisionLayers();

            // configure vision system properly using these layers
        }

        private static void ConfigureVisionLayers()
        {
            var visionLayers = new string[]
            {
                "Fog",
                "Vision",
                "Hidden"
            };

            var layersToAdd = new List<string>();

            // Open tag manager
            var tagManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");

            if (tagManagerAssets.Length == 0)
            {
                // Debug.LogWarning("Gemserk.Vision - Couldn't find TagManager.asset");
                return;
            }
            
            var tagManager =
                new SerializedObject(tagManagerAssets[0]);

            var layersProperty = tagManager.FindProperty("layers");

            foreach (var visionLayer in visionLayers)
            {
                var found = false;
                
                for (var i = 0; i < layersProperty.arraySize; i++)
                {
                    var t = layersProperty.GetArrayElementAtIndex(i);
                    if (t.stringValue.Equals(visionLayer))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // Debug.LogFormat("Gemserk.Vision - Vision layer {0} not found", visionLayer);
                    layersToAdd.Add(visionLayer);
                }
            }

            foreach (var layer in layersToAdd)
            {
                // Avoid Builtin Layers by starting in index 8
                for (var i = 8; i < layersProperty.arraySize; i++)
                {
                    var t = layersProperty.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(t.stringValue))
                    {
                        // Debug.LogFormat("Gemserk.Vision - Adding vision layer {0}", layer);
                        t.stringValue = layer;
                        break;
                    }
                }
            }
            
            tagManager.ApplyModifiedProperties();
        }
    }
}