using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class SetGameTargetFpsTriggerAction : WorldTriggerAction
    {
        public int fps = 60;
        
        public override string GetObjectName()
        {
            return $"SetGameTargetFps({fps})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            Application.targetFrameRate = fps;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}