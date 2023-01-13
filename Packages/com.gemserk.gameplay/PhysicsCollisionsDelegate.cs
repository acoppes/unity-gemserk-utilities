using UnityEngine;

namespace Gemserk.Gameplay
{
    public class PhysicsCollisionsDelegate : MonoBehaviour
    {
        public delegate void CollisionHandler(Collision collision);

        public delegate void TriggerHandler(Collider collider);

        public CollisionHandler onCollisionEnter, onCollisionExit;
        
        public TriggerHandler onTriggerEnter, onTriggerExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter != null)
            {
                onCollisionEnter(collision);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (onCollisionExit != null)
            {
                onCollisionExit(collision);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (onTriggerEnter != null)
            {
                onTriggerEnter(collider);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (onTriggerExit != null)
            {
                onTriggerExit(collider);
            }
        }
    }
}
