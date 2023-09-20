using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Definitions
{
    [Serializable]
    public class HitboxMetadata
    {
        public Sprite sprite;

        public List<HitboxAsset> hitBoxes = new();
        
        public List<HitboxAsset> hurtBoxes = new();
    }
    
    [CreateAssetMenu(menuName = "Tools/Create Sprites Metadata", fileName = "SpritesMetadata", order = 0)]
    public class SpritesMetadata : ScriptableObject
    {
        public List<HitboxMetadata> frameMetadata = new ();

        public HitboxMetadata GetFrameMetadata(Sprite sprite)
        {
            return frameMetadata
                .FirstOrDefault(f => f.sprite == sprite);
        }
    }
}