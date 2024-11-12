using Game.Scenes;
using Gemserk.Triggers;

namespace Game.Triggers
{
    public class GameSceneLoaderTriggerAction : TriggerAction
    {
        public string sceneName;
        
        public override string GetObjectName()
        {
            return $"GameSceneLoad({sceneName})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            GameSceneLoader.LoadNextScene(sceneName);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}