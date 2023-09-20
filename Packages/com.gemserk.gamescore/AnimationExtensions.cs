using Game.Definitions;
using UnityEngine;

namespace Game
{
    public static class AnimationExtensions
    {
        private const int MaxDirections = 100;
            
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