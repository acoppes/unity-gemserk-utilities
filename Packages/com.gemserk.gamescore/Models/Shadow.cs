using UnityEngine;

namespace Game.Models
{
    public class Shadow : MonoBehaviour
    {
        public SpriteRenderer shadow;

        public void ResetModel()
        {
            if (shadow != null)
            {
                shadow.transform.localScale = Vector3.one;
                shadow.transform.localEulerAngles = Vector3.zero;
            }
        }
    }
}