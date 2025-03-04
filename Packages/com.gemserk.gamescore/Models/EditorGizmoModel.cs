using System;
using UnityEngine;

namespace Game.Models
{
    public class EditorGizmoModel : MonoBehaviour
    {
        public float radius;
        
        public bool filled;
        public bool wire;
        
        private void OnDrawGizmos()
        {
            if (wire)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
            
            if (filled)
            {
                Gizmos.DrawSphere(transform.position, radius);
            }
        }
    }
}