using System.Collections.Generic;
using System.IO;
using Game.Components;
using Game.Definitions;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class AnimationCreatorTool
    {
        private class KeyFrame
        {
            public Sprite sprite;
            // public Sprite fxSprite;
            public int frame;
        }
        
        private class Animation
        {
            public List<KeyFrame> keyframes = new List<KeyFrame>();
            // public bool hasFx;
        }

        // private const float DefaultFrameRate = 15f;

        public static List<int> GetFramesFromRanges(string frameRange)
        {
            var frames = new List<int>();
            var separations = frameRange.Split("_");

            foreach (var separation in separations)
            {
                var ranges = separation.Split("-");

                if (ranges.Length == 2)
                {
                    var start = int.Parse(ranges[0]);
                    var end = int.Parse(ranges[1]);

                    for (var i = start; i <= end; i++)
                    {
                        frames.Add(i);
                    }
                } else if (ranges.Length == 1)
                {
                    frames.Add(int.Parse(ranges[0]));
                }
            }
            
            return frames;
        }

        [UnityEditor.MenuItem("Assets/Gemserk/Expand sprites in Folder")]
        public static void ExpandFiles()
        {
            var folderToExpand = EditorUtility.OpenFolderPanel("Select source folder", Application.dataPath, "");

            if (!string.IsNullOrEmpty(folderToExpand))
            {
                var targetFolder = EditorUtility.OpenFolderPanel("Select target folder", Application.dataPath, "");
                
                // var targetFolder = Path.Combine(folderToExpand, "Expanded");

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                
                var files = Directory.GetFiles(folderToExpand, "*.png");
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    
                    var fileNameParts = fileName.Split("_", 2);

                    if (fileNameParts.Length < 2)
                    {
                        continue;
                    }
                    
                    var animationName = fileNameParts[0];
                    var frameString = fileNameParts[1];

                    if (string.IsNullOrEmpty(animationName))
                    {
                        continue;
                    }
                    
                    if (string.IsNullOrEmpty(frameString))
                    {
                        continue;
                    }

                    var frames = GetFramesFromRanges(frameString);

                    foreach (var frame in frames)
                    {
                        var target = Path.Combine(targetFolder, $"{animationName}_{frame:00}.png");
                        if (!File.Exists(target))
                        {
                            FileUtil.CopyFileOrDirectory(file, target);
                        }
                    }
                }
            }
        }

        [UnityEditor.MenuItem("Assets/Gemserk/Create Animation Asset from Folder")]
        public static void CreateAnimationAssetFromFolder()
        {
            var activeObject = Selection.activeObject;

            if (activeObject == null)
            {
                // show message?
                return;
            }

            var sourceFolderPath = AssetDatabase.GetAssetPath(activeObject);
            var animationAssetName = Path.GetFileNameWithoutExtension(sourceFolderPath);
            var sprites = AssetDatabaseSpriteExtensions.GetSpritesFromFolder(sourceFolderPath);
            
            if (!AssetDatabase.IsValidFolder($"Assets/Animations"))
            {
                AssetDatabase.CreateFolder("Assets", "Animations");
            }
            
            var animationsAsset = CreateAnimationsAsset(animationAssetName, 
                AnimationDefinition.DefaultFrameRate, sprites, "Assets/Animations");
            animationsAsset.sourceFolder = sourceFolderPath;
            
            EditorUtility.SetDirty(animationsAsset);
            AssetDatabase.SaveAssets();
            
            EditorGUIUtility.PingObject(animationsAsset);
        }
        
        public static void CreateAnimationAssetFromFolder(string sourceFolderPath, string outputFolder, float defaultFps)
        {
            var animationAssetName = Path.GetFileNameWithoutExtension(sourceFolderPath);
            var sprites = AssetDatabaseSpriteExtensions.GetSpritesFromFolder(sourceFolderPath);

            // if (!Directory.Exists(outputFolderPath))
            // {
            //     Directory.CreateDirectory(outputFolderPath);
            // }

            var outputAssetsFolder = Path.Combine("Assets", outputFolder);
            
            if (!AssetDatabase.IsValidFolder(outputAssetsFolder))
            {
                AssetDatabase.CreateFolder("Assets", outputFolder);
            }
            
            var animationsAsset = CreateAnimationsAsset(animationAssetName, defaultFps, sprites, 
                outputAssetsFolder);
            animationsAsset.sourceFolder = sourceFolderPath;
            
            EditorUtility.SetDirty(animationsAsset);
            AssetDatabase.SaveAssets();
            
            EditorGUIUtility.PingObject(animationsAsset);
        }

        public static AnimationsAsset CreateAnimationsAsset(string assetName, float fps, List<Sprite> sprites, string outputFolder)
        {
            var fileName = Path.Combine(outputFolder, $"{assetName}.asset");
            var previousAnimationAsset = AssetDatabase.LoadMainAssetAtPath(fileName) as AnimationsAsset;

            var animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationsAsset.name = assetName;

            if (previousAnimationAsset && previousAnimationAsset.overrideImporterDefaultFps && previousAnimationAsset.fps > 0)
            {
                fps = previousAnimationAsset.fps;
                
                // just to keep previous info
                animationsAsset.fps = previousAnimationAsset.fps;
                animationsAsset.overrideImporterDefaultFps = previousAnimationAsset.overrideImporterDefaultFps;
            }

            ConfigureAnimationsAsset(animationsAsset, fps, sprites);

            if (previousAnimationAsset)
            {
                EditorUtility.SetDirty(animationsAsset);
                EditorUtility.CopySerialized(animationsAsset, previousAnimationAsset);
                AssetDatabase.SaveAssets();

                return previousAnimationAsset;
            }
            else
            {
                EditorUtility.SetDirty(animationsAsset);
                AssetDatabase.CreateAsset(animationsAsset, fileName);
            }

            return animationsAsset;
        }
        
        public static void ConfigureAnimationsAsset(AnimationsAsset animationsAsset, float defaultFps, List<Sprite> sprites)
        {
            // TODO: don't override animations metadata in asset like events
            
            animationsAsset.animations.Clear();
            
            var animations = new Dictionary<string, Animation>();

            foreach (var sprite in sprites)
            {
                var spriteParts = sprite.name.Split("_");
                var animationName = spriteParts[^2];
                var frameString = spriteParts[^1];

                // if (animationName.EndsWith("fx", StringComparison.OrdinalIgnoreCase))
                // {
                //     continue;
                //     // animationName = animationName.Replace("fx", "");
                // }
                
                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new Animation();
                }

                var animation = animations[animationName];

                // var fxSprite = sprites.Find(s =>
                //     s.name.Equals($"{animationName}fx_{frameString}", StringComparison.OrdinalIgnoreCase));

                var frames = GetFramesFromRanges(frameString);

                foreach (var frame in frames)
                {
                    animation.keyframes.Add(new KeyFrame
                    {
                        sprite = sprite,
                        frame = frame
                    });
                }
            }

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];
                animation.keyframes.Sort((a, b) => a.frame - b.frame);
            }

            var frameTime = 1.0f / defaultFps;

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];

                var animationDefinition = new AnimationDefinition
                {
                    name = animationName
                };

                for (var i = 0; i < animation.keyframes.Count; i++)
                {
                    animationDefinition.frames.Add(new AnimationFrame()
                    {
                        sprite = animation.keyframes[i].sprite,
                        time = frameTime
                    });
                }

                animationDefinition.duration = animation.keyframes.Count * frameTime;
                animationsAsset.animations.Add(animationDefinition);
            }
            
            EditorUtility.SetDirty(animationsAsset);
            AssetDatabase.SaveAssets();
        }
    }
}