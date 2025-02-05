using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gemserk.Triggers
{
    public class SendMessageEntityTriggerAction : WorldTriggerAction
    {
        public enum Type
        {
            Send = 0,
            Broadcast = 1
        }

        public TriggerTarget target;
        
        public string methodName;
        public SendMessageOptions sendMessageOptions = SendMessageOptions.DontRequireReceiver;
        public Type type;
        public bool sendActivator;
        
        public override string GetObjectName()
        {
            if (target == null)
            {
                return "Send(not configured)";
            }
            
            if (type == Type.Send)
            {
                return $"SendMessage({methodName}, {target})";
            }
            else
            {
                return $"Broadcast({methodName}, {target})";
            }
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = new List<Entity>();
            target.Get(targets, world, activator);

            foreach (var target in targets)
            {
                var gameObjectComponent = target.Get<GameObjectComponent>();
                
                if (type == Type.Send)
                {
                    if (!sendActivator)
                    {
                        gameObjectComponent.gameObject.SendMessage(methodName, sendMessageOptions);
                    }
                    else
                    {
                        Assert.IsNotNull(activator, "Cant send message with null activator");
                        gameObjectComponent.gameObject.SendMessage(methodName, activator, sendMessageOptions);
                    }
                }
                else
                {
                    if (!sendActivator)
                    {
                        gameObjectComponent.gameObject.BroadcastMessage(methodName, sendMessageOptions);
                    }
                    else
                    {
                        Assert.IsNotNull(activator, "Cant send message with null activator");
                        gameObjectComponent.gameObject.BroadcastMessage(methodName, activator, sendMessageOptions);
                    }
                }
            }

            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}