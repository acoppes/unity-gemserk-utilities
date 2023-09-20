using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Definitions
{
    [Serializable]
    public class CameraShake
    {
        public float duration;
        public Vector2 magnitude;
        public AnimationCurve decay = AnimationCurve.Linear(0, 1, 1, 0);
        
        public static IEnumerator Shake(CameraShake cameraShake, Transform t)
        {
            t.position = Vector3.zero;

            var elapsed = 0f;

            var x = cameraShake.magnitude.x * (Random.Range(-1f, 1f) > 0 ? 1f : -1f);
            var y = cameraShake.magnitude.y * (Random.Range(-1f, 1f) > 0 ? 1f : -1f);
            
            while (elapsed < cameraShake.duration)
            {
                t.position = new Vector3(x, y, 0);
                yield return new WaitForSeconds(1f/15f);
                elapsed += 1f / 15f;

                var time = elapsed / cameraShake.duration;
                var decayValue = cameraShake.decay.Evaluate(time);

                x *= -decayValue;
                y *= -decayValue;
            }
            
            t.position = Vector3.zero;
        }
    }
    
    [CreateAssetMenu(menuName = "Tools/Create Camera Shake", fileName = "CameraShake", order = 0)]
    public class CameraShakeAsset : ScriptableObject
    {
        public CameraShake shake;
    }
}