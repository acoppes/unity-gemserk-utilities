using System.Collections.Generic;
using Unity.Cinemachine;
using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
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
            StopFollowingAll = 1,
            StopFollowingTarget = 2
        }
        
        public ActionType actionType;
        
        public Query query;

        public TriggerTarget target;

        public GameObject targetGameObject;
        
        public string cameraName;

        public bool useModel;
        public bool forceCameraPosition;

        public bool useGameObject;

        public bool resetPreviousFollow = true;
        
        public override string GetObjectName()
        {
            if (query)
            {
                return $"SetCamera{actionType}({cameraName}, {query})";
            }    
            return $"SetCamera{actionType}({cameraName}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var cameraObject = GameObject.Find(cameraName);
            var virtualCamera = cameraObject.GetComponent<CinemachineCamera>();

            if (virtualCamera && resetPreviousFollow)
            {
                CinemachineCameraUtils.ClearTargets(virtualCamera);   
            }
            
            if (actionType == ActionType.Follow)
            {
                if (targetGameObject)
                {
                    CinemachineCameraUtils.FollowTarget(virtualCamera, targetGameObject.transform);
                    CinemachineCameraUtils.LookAtTarget(virtualCamera, targetGameObject.transform);
                }
                else
                {
                    var entities = new List<Entity>();

                    if (query)
                    {
                        entities = world.GetEntities(query.GetEntityQuery());
                    }
                    else
                    {
                        target.Get(entities, world, activator);
                    }
                    
                    foreach (var entity in entities)
                    {
                        if (useGameObject)
                        {
                            var gameObjectComponent = world.GetComponent<GameObjectComponent>(entity);

                            CinemachineCameraUtils.FollowTarget(virtualCamera, gameObjectComponent.gameObject.transform);
                            CinemachineCameraUtils.LookAtTarget(virtualCamera, gameObjectComponent.gameObject.transform);
                            
                        }
                        else
                        {
                            var modelComponent = world.GetComponent<ModelComponent>(entity);
                    
                            if (!useModel)
                            {
                                CinemachineCameraUtils.FollowTarget(virtualCamera, modelComponent.instance.transform);
                                CinemachineCameraUtils.LookAtTarget(virtualCamera, modelComponent.instance.transform);
                            }
                            else
                            {
                                CinemachineCameraUtils.FollowTarget(virtualCamera, modelComponent.instance.model);
                                CinemachineCameraUtils.LookAtTarget(virtualCamera, modelComponent.instance.model);
                            }
                        }
                        
                    }
                }
                
                if (forceCameraPosition)
                {
                    virtualCamera.ForceCameraPosition(virtualCamera.Follow.transform.position, Quaternion.identity);
                }
                
            } else if (actionType == ActionType.StopFollowingAll)
            {
                virtualCamera.Follow = null;
                virtualCamera.LookAt = null;
            } else if (actionType == ActionType.StopFollowingTarget)
            {
                if (targetGameObject)
                {
                    CinemachineCameraUtils.ClearTarget(virtualCamera, targetGameObject.transform);
                }
                else
                {
                    var entities = new List<Entity>();

                    if (query)
                    {
                        entities = world.GetEntities(query.GetEntityQuery());
                    }
                    else
                    {
                        target.Get(entities, world, activator);
                    }
                    
                    foreach (var entity in entities)
                    {
                        if (useGameObject)
                        {
                            var gameObjectComponent = world.GetComponent<GameObjectComponent>(entity);

                            CinemachineCameraUtils.ClearTarget(virtualCamera, gameObjectComponent.gameObject.transform);
                            
                        }
                        else
                        {
                            var modelComponent = world.GetComponent<ModelComponent>(entity);
                    
                            if (!useModel)
                            {
                                CinemachineCameraUtils.ClearTarget(virtualCamera, modelComponent.instance.transform);
                            }
                            else
                            {
                                CinemachineCameraUtils.ClearTarget(virtualCamera, modelComponent.instance.model);
                            }
                        }
                        
                    }
                }
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}