using UnityEngine;

namespace Gemserk.Utilities
{
    public class PhysicsCollisionsDelegate : MonoBehaviour
    {
        public delegate void CollisionHandler(Collision collision);

        public delegate void TriggerHandler(Collider collider);

        public CollisionHandler onCollisionEnter, onCollisionStay, onCollisionExit;
        
        public TriggerHandler onTriggerEnter, onTriggerStay, onTriggerExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter != null)
            {
                onCollisionEnter(collision);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (onCollisionStay != null)
            {
                onCollisionStay(collision);
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

        private void OnTriggerStay(Collider collider)
        {
            if (onTriggerStay != null)
            {
                onTriggerStay(collider);
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
