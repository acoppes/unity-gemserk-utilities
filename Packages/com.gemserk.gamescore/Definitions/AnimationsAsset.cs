using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using UnityEngine;

namespace Game.Definitions
{
    [CreateAssetMenu(menuName = "Gemserk/Animation")]
    public class AnimationsAsset : ScriptableObject
    {
        public string sourceFolder;
        
        public List<AnimationDefinition> animations = new();
        
        public IDictionary<string, int> GetAnimationsByNameDictionary()
        {
            var cachedAnimations = new Dictionary<string, int>();
            
            for (var i = 0; i < animations.Count; i++)
            {
                var animation = animations[i];
                cachedAnimations[animation.name] = i;
            }

            return cachedAnimations;
        }
        
        public int GetAnimationIndexByName(string animationName)
        {
            for (var i = 0; i < animations.Count; i++)
            {
                var animation = animations[i];
                if (animation.name.Equals(animationName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }
        
        public bool HasAnimation(string animationName)
        {
            return animations.Any(a => a.name.Equals(animationName, StringComparison.OrdinalIgnoreCase));
        }
    }
}