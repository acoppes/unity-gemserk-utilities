using Unity.Cinemachine;
using UnityEngine;

namespace Game.Utilities
{
    public static class CinemachineCameraUtils
    {
        public static void ClearTargets(CinemachineCamera camera)
        {
            if (camera.Follow)
            {
                var targetGroup = camera.Follow.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.Targets.Clear();
                }
            }
            
            if (camera.LookAt)
            {
                var targetGroup = camera.LookAt.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.Targets.Clear();
                }
            }
        }
        
        public static void ClearTarget(CinemachineCamera camera, Transform targetTransform)
        {
            if (camera.Follow)
            {
                var targetGroup = camera.Follow.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.Targets.RemoveAll(t => t.Object == targetTransform);
                }
            }
            
            if (camera.LookAt)
            {
                var targetGroup = camera.LookAt.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.Targets.RemoveAll(t => t.Object == targetTransform);
                }
            }
        }
        
        public static void FollowTarget(CinemachineCamera camera, Transform followTransform)
        {
            if (camera.Follow)
            {
                var targetGroup = camera.Follow.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.AddMember(followTransform, 1, 0);
                }
            }
            else
            {
                camera.Follow = followTransform;
            }
        }
        
        public static void LookAtTarget(CinemachineCamera camera, Transform followTransform)
        {
            if (camera.LookAt)
            {
                var targetGroup = camera.LookAt.GetComponent<CinemachineTargetGroup>();
                if (targetGroup)
                {
                    targetGroup.AddMember(followTransform, 1, 0);
                }
            }
            else
            {
                camera.LookAt = followTransform;
            }
        }
        
    }
}