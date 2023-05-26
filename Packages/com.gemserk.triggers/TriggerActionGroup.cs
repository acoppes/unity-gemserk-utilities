using System.Collections.Generic;
using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    public class TriggerActionGroup : TriggerAction
    {
        public string groupName;
        
        private readonly List<TriggerAction> actions = new();

        private int runningActionIndex;

        public override string GetObjectName()
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return "Actions()";
            }
            
            return groupName;
        }

        private void Awake()
        {
            gameObject.GetComponentsInChildrenDepth1(false, true, actions);
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            for (var i = runningActionIndex; i < actions.Count; i++)
            {
                var action = actions[i];
                var result = action.Execute(activator);

                if (result == ITrigger.ExecutionResult.Running)
                {
                    runningActionIndex = i;
                    return ITrigger.ExecutionResult.Running;
                }
            }

            runningActionIndex = 0;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}