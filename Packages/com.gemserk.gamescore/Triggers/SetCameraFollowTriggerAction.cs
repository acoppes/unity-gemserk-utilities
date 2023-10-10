using Cinemachine;
using Game.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Triggers
{
    public class SetCameraFollowTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Follow = 0,
            StopFollowing = 1
        }
        
        public ActionType actionType;
        
        public Query query;
        
        public string cameraName;

        public bool useModel;
        public bool forceCameraPosition;

        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"SetCamera{actionType}({cameraName}, {query})";
            }    
            return $"SetCamera{actionType}()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var cameraObject = GameObject.Find(cameraName);
            var virtualCamera = cameraObject.GetComponent<CinemachineVirtualCamera>();
            
            if (actionType == ActionType.Follow)
            {
                var entities = world.GetEntities(query.GetEntityQuery());
                
                foreach (var entity in entities)
                {
                    var modelComponent = world.GetComponent<ModelComponent>(entity);
                    
                    if (!useModel)
                    {
                        virtualCamera.Follow = modelComponent.instance.transform;
                        virtualCamera.LookAt = modelComponent.instance.transform;
                    }
                    else
                    {
                        virtualCamera.Follow = modelComponent.instance.model;
                        virtualCamera.LookAt = modelComponent.instance.model;
                    }

                    if (forceCameraPosition)
                    {
                        virtualCamera.ForceCameraPosition(virtualCamera.Follow.transform.position, Quaternion.identity);
                    }
                }
            } else if (actionType == ActionType.StopFollowing)
            {
                virtualCamera.Follow = null;
                virtualCamera.LookAt = null;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}