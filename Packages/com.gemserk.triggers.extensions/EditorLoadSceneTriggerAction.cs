using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Triggers
{
    public class EditorLoadSceneTriggerAction : TriggerAction
    {
        public string sceneName;
        
        public override string GetObjectName()
        {
            return $"EditorLoadScene({sceneName})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(sceneName, new LoadSceneParameters(LoadSceneMode.Single));
                return ITrigger.ExecutionResult.Completed;
            }
#endif
            return ITrigger.ExecutionResult.Completed;
        }
    }
}