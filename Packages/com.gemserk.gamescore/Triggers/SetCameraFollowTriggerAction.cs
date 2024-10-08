﻿using System.Collections.Generic;
using Unity.Cinemachine;
using Game.Components;
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
            StopFollowing = 1
        }
        
        public ActionType actionType;
        
        public Query query;

        public TriggerTarget target;

        public GameObject targetGameObject;
        
        public string cameraName;

        public bool useModel;
        public bool forceCameraPosition;

        public bool useGameObject;
        
        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"SetCamera{actionType}({cameraName}, {query})";
            }    
            return $"SetCamera{actionType}({cameraName}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var cameraObject = GameObject.Find(cameraName);
            var virtualCamera = cameraObject.GetComponent<CinemachineCamera>();
            
            if (actionType == ActionType.Follow)
            {
                if (targetGameObject != null)
                {
                    virtualCamera.Follow = targetGameObject.transform;
                    virtualCamera.LookAt = targetGameObject.transform;
                }
                else
                {
                    var entities = new List<Entity>();

                    if (query != null)
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
                            virtualCamera.Follow = gameObjectComponent.gameObject.transform;
                            virtualCamera.LookAt = gameObjectComponent.gameObject.transform;
                        }
                        else
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
                        }
                        
                    }
                }
                
                if (forceCameraPosition)
                {
                    virtualCamera.ForceCameraPosition(virtualCamera.Follow.transform.position, Quaternion.identity);
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