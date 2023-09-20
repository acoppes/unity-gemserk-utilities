using Cinemachine;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class CinemachineTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            SetConfiner = 0,
            SetNoise = 1
        }

        public ActionType actionType = ActionType.SetConfiner;
        
        public string cameraName;
        public string cameraConfinerName;

        public NoiseSettings noiseSettings;

        public override string GetObjectName()
        {
            if (actionType == ActionType.SetConfiner)
            {
                return $"SetCameraConfiner({cameraName}, {cameraConfinerName})";
            }
            else
            {
                if (noiseSettings != null)
                {
                    return $"SetNoiseSettings({cameraName}, {noiseSettings.name})";
                }
                return $"UnsetNoiseSettings({cameraName})";
            }

            return "Cinemachine()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var cameraObject = GameObject.Find(cameraName);
            if (cameraObject == null)
            {
                return ITrigger.ExecutionResult.Running;
            }
            
            var virtualCamera = cameraObject.GetComponent<CinemachineVirtualCamera>();

            if (actionType == ActionType.SetConfiner)
            {
                var cameraConfiner = GameObject.Find(cameraConfinerName);
                var cinemachineConfiner2D = virtualCamera.GetComponent<CinemachineConfiner2D>();

                if (cameraConfiner == null)
                {
                    cinemachineConfiner2D.m_BoundingShape2D = null;
                    cinemachineConfiner2D.enabled = false;
                }
                else
                {
                    cinemachineConfiner2D.enabled = true;
                    cinemachineConfiner2D.m_BoundingShape2D =
                        cameraConfiner.GetComponentInChildren<Collider2D>();
                    cinemachineConfiner2D.InvalidateCache();
                }
            } else if (actionType == ActionType.SetNoise)
            {
                var perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                
                // if (perlinNoise == null)
                // {
                //     perlinNoise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // }
                //
                perlinNoise.m_NoiseProfile = noiseSettings;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}