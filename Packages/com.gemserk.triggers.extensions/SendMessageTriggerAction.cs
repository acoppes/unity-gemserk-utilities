using Gemserk.Triggers.Queries;
using Gemserk.Utilities.Messaging;

namespace Gemserk.Triggers
{
    public class SendMessageTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        public MessageSystem.MessageInfo messageInfo;
        
        public override string GetObjectName()
        {
            return $"SendMessage({messageInfo.message}, {messageInfo.parameter})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);

            foreach (var targetEntity in targets)
            {
                MessageSystem.SendMessage(targetEntity, messageInfo.message, messageInfo.parameter);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}