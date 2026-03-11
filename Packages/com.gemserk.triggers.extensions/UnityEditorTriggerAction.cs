namespace Gemserk.Triggers
{
    public class UnityEditorTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Stop = 0,
            Pause = 1,
        }

        public ActionType actionType = ActionType.Stop;

        public override string GetObjectName()
        {
            return $"UnityEditor.{actionType}()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (actionType == ActionType.Pause)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = true;
#endif
            }
            
            if (actionType == ActionType.Stop)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}