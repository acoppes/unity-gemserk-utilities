using System.Globalization;
using UnityEngine;

namespace Gemserk.Triggers.Actions
{
    public class SetTimeScaleTriggerAction : TriggerAction
    {
        public float timeScale = 1;

        public override string GetObjectName()
        {
            return $"SetTimeScale({timeScale.ToString(CultureInfo.InvariantCulture)})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            Time.timeScale = timeScale;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}