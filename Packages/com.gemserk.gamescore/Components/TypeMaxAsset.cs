using UnityEngine;

namespace Game.Components
{
    [CreateAssetMenu(menuName = "Gemserk/Type Max", order = 0)]
    public class TypeMaxAsset : ScriptableObject, ITypeMax
    {
        public int max;
        public int GetMax()
        {
            return max;
        }
    }
}