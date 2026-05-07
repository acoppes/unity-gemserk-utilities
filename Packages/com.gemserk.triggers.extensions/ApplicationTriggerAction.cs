using UnityEngine;

namespace Gemserk.Triggers
{
    public class ApplicationTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Quit = 0
        }

        public ActionType actionType = ActionType.Quit;

        public override string GetObjectName()
        {
            return $"Application.{actionType}()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}