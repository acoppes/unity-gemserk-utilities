using Game.Scenes;
using UnityEngine;

namespace Game.Screens
{
    public class NotesSceneController : MonoBehaviour
    {
        public static TextAsset notesText;
        
        public string nextSceneName = "MainMenu";

        public NotesScreen notes;

        private void Start()
        {
            if (notesText != null)
            {
                notes.LoadText(notesText);
            }
            
            notes.onClose += delegate
            {
                GameSceneLoader.LoadNextScene(nextSceneName);
            };
        }
    }
}