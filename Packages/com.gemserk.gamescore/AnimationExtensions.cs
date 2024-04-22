using System.Collections.Generic;
using Game.Components;
using Game.Definitions;
using UnityEngine;

namespace Game
{
    public static class AnimationExtensions
    {
        private const int MaxDirections = 100;

        public static AnimationsDirectionsMetadata GetDirectionsData(this AnimationsAsset asset)
        {
            var metadata = new AnimationsDirectionsMetadata();

            foreach (var animationDefinition in asset.animations)
            {
                var animationNameParts = animationDefinition.name.Split("-");
                var animationName = animationNameParts[0];
                
                if (!metadata.animations.ContainsKey(animationName))
                {
                    metadata.animations[animationName] = new AnimationDirectionMetadata();
                }

                if (animationNameParts.Length == 1)
                {
                    metadata.animations[animationName].animationNames.Add($"{animationName}");
                    metadata.animations[animationName].directions = 1;
                }
                else
                {
                    metadata.animations[animationName].animationNames.Add($"{animationName}-{metadata.animations[animationName].directions}");
                    metadata.animations[animationName].directions++;
                }
            }

            return metadata;
        }
            
        public static string GetDirectionalAnimation(this AnimationsAsset asset, string animationName,
            Vector2 direction)
        {
            // TODO: cache this somewhere...
            
            var countDirections = 0;

            if (direction.x < 0)
            {
                direction.x *= -1;
            }

            do
            {
                var name = $"{animationName}-{countDirections}";
                if (asset.HasAnimation(name))
                {
                    countDirections++;
                }
                else
                {
                    break;
                }
            } while (countDirections < MaxDirections);

            if (countDirections > 0)
            {
                var anglePerDirection = 180 / countDirections;
                var currentAngle = -90;
                var nextAngle = currentAngle + anglePerDirection;
                
                for (int i = 0; i < countDirections; i++)
                {
                    var angle = Vector2.SignedAngle(Vector2.right, direction);

                    if (angle >= currentAngle && angle <= nextAngle)
                    {
                        return $"{animationName}-{i}";
                    }

                    currentAngle += anglePerDirection;
                    nextAngle += anglePerDirection;
                }
            }

            return animationName;
        }   
    }
}