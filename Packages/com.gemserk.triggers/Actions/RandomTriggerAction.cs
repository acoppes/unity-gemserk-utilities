using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities;
using Random = UnityEngine.Random;

namespace Gemserk.Triggers.Actions
{
    [TriggerEditor("Random Action")]
    public class RandomTriggerAction : TriggerAction
    {
        private List<TriggerAction> triggerActions = new();

        private ITrigger.IAction randomAction;

        private void Awake()
        {
            gameObject.GetComponentsInChildren(false, true, 1, triggerActions);
            // GetComponentsInChildren(false, triggerActions);
            // triggerActions.Remove(this);
        }

        public override string GetObjectName()
        {
            if (triggerActions != null && triggerActions.Count > 0)
            {
                return $"Random({string.Join(',', triggerActions.Select(t => t.name))})";
            }
            return "Random()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (triggerActions.Count == 0)
            {
                return ITrigger.ExecutionResult.Completed;
            }
            
            if (randomAction == null)
            {
                randomAction = triggerActions[Random.Range(0, triggerActions.Count)].GetInterface<ITrigger.IAction>();
            }

            var result = randomAction.Execute(activator);

            if (result == ITrigger.ExecutionResult.Completed)
            {
                randomAction = null;
            }

            return result;
        }
    }
}