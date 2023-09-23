using UnityEngine;
using UnityEngine.Assertions;

namespace Gemserk.Triggers
{
    public class SendMessageTriggerAction : TriggerAction
    {
        public enum Type
        {
            Send = 0,
            Broadcast = 1
        }

        public GameObject target;
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
                return $"SendMessage({methodName}, {target.name})";
            }
            else
            {
                return $"Broadcast({methodName}, {target.name})";
            }
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (type == Type.Send)
            {
                if (!sendActivator)
                {
                    target.SendMessage(methodName, sendMessageOptions);
                }
                else
                {
                    Assert.IsNotNull(activator, "Cant send message with null activator");
                    target.SendMessage(methodName, activator, sendMessageOptions);
                }
            }
            else
            {
                if (!sendActivator)
                {
                    target.BroadcastMessage(methodName, sendMessageOptions);
                }
                else
                {
                    Assert.IsNotNull(activator, "Cant send message with null activator");
                    target.BroadcastMessage(methodName, activator, sendMessageOptions);
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}