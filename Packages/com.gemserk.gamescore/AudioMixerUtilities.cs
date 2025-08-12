using UnityEngine;

namespace Game
{
    public static class AudioMixerUtilities
    {
        public static float ConvertFromDbToNormalizedValue(float dbValue)
        {
            return Mathf.Exp(dbValue / 20f);
        }
        
        public static float ConvertFromNormalizedValueToDb(float value)
        {
            return Mathf.Log(value) * 20;
        }
        
        public static float ConvertFromNormalizedValueToDb(float value, float min, float max)
        {
            var newValue = Mathf.Lerp(min, max,value);
            return Mathf.Log(newValue) * 20;
        }
        
        public static float ConvertFromDbToNormalizedValue(float dbValue, float min, float max)
        {
            var storedValue = Mathf.Exp(dbValue / 20f);
            return Mathf.InverseLerp(min, max, storedValue);
        }
    }
}