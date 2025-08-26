using System.Collections.Generic;
using Gemserk.Utilities;

namespace Gemserk.Triggers
{
    [TriggerEditor("Actions Group")]
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
                
                var result = ITrigger.ExecutionResult.Completed;
                // will assume disabled actions are ignored for now.
                if (!action.Disabled)
                {
                    result = action.Execute(activator);
                }
                
                if (result == ITrigger.ExecutionResult.Running || result == ITrigger.ExecutionResult.Interrupt)
                {
                    runningActionIndex = i;
                    return result;
                }
            }

            runningActionIndex = 0;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}