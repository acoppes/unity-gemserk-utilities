using Gemserk.Utilities.Messaging;

namespace Gemserk.Triggers
{
    public class OnMessageTriggerEvent : TriggerEvent, MessageSystem.IMessageListener
    {
        public MessageSystem.MessageInfo messageInfo;
        
        public override string GetObjectName()
        {
            return $"OnMessage({messageInfo.message}, {messageInfo.parameter})";
        }

        private void OnEnable()
        {
            MessageSystem.RegisterListener(this, messageInfo.message, messageInfo.parameter);
        }

        private void OnDisable()
        {
            MessageSystem.UnregisterListener(this, messageInfo.message, messageInfo.parameter);
        }

        public void OnMessage(MessageSystem.MessageArgs messageArgs)
        {
            trigger.QueueExecution(messageArgs.sender);
        }
    }
}