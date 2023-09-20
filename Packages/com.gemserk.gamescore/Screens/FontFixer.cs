using UnityEngine;

namespace Game.Screens
{
    public class FontFixer : MonoBehaviour
    {
        public Font font;

        private void Awake()
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}