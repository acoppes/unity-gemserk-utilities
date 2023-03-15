using UnityEngine;

namespace Gemserk.Utilities
{
    public class Physics2dCollisionsDelegate : MonoBehaviour
    {
        public delegate void Collision2dHandler(Collision2D collision);
        public delegate void Trigger2dHandler(Collider2D collider);

        public Collision2dHandler onCollisionEnter2d, onCollisionStay2d, onCollisionExit2d;
        public Trigger2dHandler onTriggerEnter2d, onTriggerStay2d, onTriggerExit2d;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (onCollisionEnter2d != null)
            {
                onCollisionEnter2d(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (onCollisionStay2d != null)
            {
                onCollisionStay2d(collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (onCollisionExit2d != null)
            {
                onCollisionExit2d(collision);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (onTriggerEnter2d != null)
            {
                onTriggerEnter2d(collider);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (onTriggerStay2d != null)
            {
                onTriggerStay2d(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (onTriggerExit2d != null)
            {
                onTriggerExit2d(collider);
            }
        }
    }
}