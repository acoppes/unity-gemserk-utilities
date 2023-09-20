using UnityEngine;
using Vertx.Debugging;

namespace Game.LevelDesign
{
    public class DebugLabelObject : MonoBehaviour
    {
        public bool disableShowName;
     
        private void OnDrawGizmos()
        {
            if (!disableShowName)
            {
                D.raw(new Shape.Text(transform.position, gameObject.name));
            }
        }
    }
}