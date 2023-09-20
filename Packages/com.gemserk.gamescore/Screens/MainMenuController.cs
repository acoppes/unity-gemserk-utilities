using Game.Scenes;
using UnityEngine;

namespace Game.Screens
{
    public class MainMenuController : MonoBehaviour
    {
        public string gameSceneName = "Game";

        public string notesSceneName = "Notes";

        public TextAsset changelog;
        public TextAsset roadmap;
        
        public void StartGameWithPlayers(int players)
        {
            GameSceneController.players = players;
            GameSceneLoader.LoadNextScene(gameSceneName);
            // StartCoroutine(GameSceneLoader.LoadNextScene(gameSceneName));
        }

        public void ShowChangelog()
        {
            NotesSceneController.notesText = changelog;
            GameSceneLoader.LoadNextScene(notesSceneName);
            //StartCoroutine(LoadingSceneController.LoadNextScene(notesSceneName));
        }
        
        public void ShowRoadmap()
        {
            NotesSceneController.notesText = roadmap;
            GameSceneLoader.LoadNextScene(notesSceneName);
            // StartCoroutine(LoadingSceneController.LoadNextScene(notesSceneName));
        }
    }
}
