using UnityEngine;

namespace Gemserk.Actions
{
    public abstract class TriggerAction : MonoBehaviour, ITrigger.IAction
    {
        public abstract ITrigger.ExecutionResult Execute();
    }
}