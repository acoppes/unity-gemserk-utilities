using UnityEngine.SceneManagement;

namespace Gemserk.Triggers
{
    public class LoadSceneTriggerAction : TriggerAction
    {
        public string sceneName;
        
        public override string GetObjectName()
        {
            return $"LoadScene({sceneName})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            SceneManager.LoadScene(sceneName);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}