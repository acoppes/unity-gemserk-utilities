using UnityEngine;

namespace Game.Scenes
{
    public class LoadingSceneController : MonoBehaviour
    {
        public string nextSceneName = "Game";

        public bool disableAutoLoadOnStart;

        private void Start()
        {
            if (!disableAutoLoadOnStart)
            {
                GameSceneLoader.LoadNextScene(nextSceneName);
            }
        }
    }
}