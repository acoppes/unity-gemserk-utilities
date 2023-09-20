﻿using Game.Systems;
using Gemserk.Utilities;
using UnityEngine;

namespace Game
{
    public static class RandomExtensions
    {
        public static bool RandomBool()
        {
            return UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f;
        }
        
        public static Vector2 RandomVector2(float minLength, float maxLength, float minAngle, float maxAngle)
        {
            return new Vector2(1, 0).Rotate(UnityEngine.Random.Range(minAngle, maxAngle)* Mathf.Deg2Rad) 
                   * UnityEngine.Random.Range(minLength, maxLength);
        }
        
        public static Vector3 RandomVectorXZ(float minLength, float maxLength, float minAngle = 0, float maxAngle = 360)
        {
            var p =  new Vector2(1, 0).Rotate(UnityEngine.Random.Range(minAngle, maxAngle)* Mathf.Deg2Rad) 
                     * UnityEngine.Random.Range(minLength, maxLength);
            return GamePerspective.ConvertToWorld(p);
        }
        
        public static Vector2 RandomInsideQuad(Vector2 size)
        {
            var result = Vector2.zero;
            
            result.x = UnityEngine.Random.Range(-size.x * 0.5f, size.x * 0.5f);
            result.y = UnityEngine.Random.Range(-size.y * 0.5f, size.y * 0.5f);
            
            return result;
        }
    }
}