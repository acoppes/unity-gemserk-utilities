using UnityEngine;

namespace Gemserk.Actions
{
    public class TriggerObject : MonoBehaviour, ITrigger
    {
        private readonly Trigger trigger = new Trigger();

        public ITrigger.ExecutionState State => trigger.State;
        
        private void Awake()
        {
            GetComponentsInChildren(false, trigger.events);
            GetComponentsInChildren(false, trigger.conditions);
            GetComponentsInChildren(false, trigger.actions);
        }

        public bool Evaluate()
        {
            return trigger.Evaluate();
        }

        public ITrigger.ExecutionResult Execute()
        {
            return trigger.Execute();
        }

        public void StartExecution()
        {
            trigger.StartExecution();
        }

        public void StopExecution()
        {
            trigger.StopExecution();
        }

        private void OnValidate()
        {
            transform.FindOrCreateFolder("Events");
            transform.FindOrCreateFolder("Conditions");
            transform.FindOrCreateFolder("Actions");
        }
    }
}