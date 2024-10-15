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
    }
}