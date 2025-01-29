using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    public static class GameSceneLoader
    {
        private class CoroutineController : MonoBehaviour
        {
            
        }
        
        private static CoroutineController _loadingController;

        private static readonly string LoadingSceneName = "LoadingScene";

        private static void CreateController()
        {
            if (_loadingController == null)
            {
                var loadingControllerObject = new GameObject("~LoadingScenesController");
                Object.DontDestroyOnLoad(loadingControllerObject);
                _loadingController = loadingControllerObject.AddComponent<CoroutineController>();
            }
        }

        private static IEnumerator LoadScene(string name)
        {
            SceneManager.LoadScene(LoadingSceneName);
            
            yield return null;
            var sceneAsync = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return sceneAsync;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));

            yield return SceneManager.UnloadSceneAsync(LoadingSceneName);
        }
        
        public static void LoadNextScene(string name)
        {
            CreateController();
            _loadingController.StartCoroutine(LoadScene(name));
        }
        
        public static void LoadMainMenu()
        {
            CreateController();
            _loadingController.StartCoroutine(LoadScene("MainMenu"));
        }
        
        public static void LoadTutorial()
        {
            CreateController();
            _loadingController.StartCoroutine(LoadScene("Tutorial"));
        }
        
        public static void Restart()
        {
            CreateController();
            _loadingController.StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}