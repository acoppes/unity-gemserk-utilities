using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        public enum FollowType
        {
            Both = 0,
            OnlyX = 1
        }
        
        public Transform followTransform;
        public Transform cameraTransform;

        public FollowType followType;

        public Vector2 offset;

        private void Update()
        {
            if (cameraTransform == null || followTransform == null) 
                return;
            
            var p = cameraTransform.transform.position;
            p.x = followTransform.position.x + offset.x;
            
            if (followType != FollowType.OnlyX)
            {
                p.y = followTransform.position.y + offset.y;
            }
            
            cameraTransform.transform.position = p;

        }
    }
}