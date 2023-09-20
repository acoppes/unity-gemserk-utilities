using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Triggers
{
    public class Collider2dTriggerEvent : TriggerEvent
    {
        [Flags]
        public enum Type
        {
            None = 0,
            OnEnter = 1 << 0,
            OnExit = 1 << 1
        }

        public Type type;
        
        public Collider2D regionCollider;

        private Physics2dCollisionsDelegate physics2dCollisionsDelegate;
        
        public override string GetObjectName()
        {
            return $"OnRegionCollider()";
        }

        private void Awake()
        {
            physics2dCollisionsDelegate = regionCollider.GetComponent<Physics2dCollisionsDelegate>();
            Assert.IsNotNull(physics2dCollisionsDelegate, "Need a collisions delegate in order to work");
        }
        
        private void OnEnable()
        {
            if (physics2dCollisionsDelegate != null)
            {
                physics2dCollisionsDelegate.onTriggerEnter2d += OnTriggerEnter2d;
                physics2dCollisionsDelegate.onTriggerExit2d += OnTriggerExit2d;
            }
        }
        
        private void OnDisable()
        {
            if (physics2dCollisionsDelegate != null)
            {
                physics2dCollisionsDelegate.onTriggerEnter2d -= OnTriggerEnter2d;
                physics2dCollisionsDelegate.onTriggerExit2d -= OnTriggerExit2d;
            }
        }
        private void OnTriggerEnter2d(Collider2D collider)
        {
            if ((type & Type.OnEnter) == Type.OnEnter)
            {
                var entityReference = collider.GetComponentInParent<EntityReference>();
                if (entityReference != null)
                {
                    trigger.QueueExecution(entityReference.entity);   
                }
                else
                {
                    trigger.QueueExecution();    
                }
            }
        }
        
        private void OnTriggerExit2d(Collider2D collider)
        {
            if ((type & Type.OnExit) == Type.OnExit)
            {
                var entityReference = collider.GetComponentInParent<EntityReference>();
                if (entityReference != null)
                {
                    trigger.QueueExecution(entityReference.entity);   
                }
                else
                {
                    trigger.QueueExecution();    
                }
            }
        }


        
    }
}