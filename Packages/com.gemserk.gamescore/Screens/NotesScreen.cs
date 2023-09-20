using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Screens
{
    public class NotesScreen : MonoBehaviour
    {
        public InputAction nextPage;
        
        [FormerlySerializedAs("previousPage")] 
        public InputAction closeAction;
        
        [FormerlySerializedAs("changelogText")] 
        public TextView text;

        public GameObject nextPageObject;

        [FormerlySerializedAs("changelogFile")] 
        public TextAsset file;
        
        private List<string> pages = new List<string>();

        private int currentPage = 0;

        public event Action onClose;

        private bool closed;

        public float touchDelay = 1.0f;
        private float touchDelayCurrent;
        
        private void OnEnable()
        {
            nextPage.Enable();
            closeAction.Enable();
        }
        
        private void Start()
        {
            LoadText(file);
            touchDelayCurrent = touchDelay;
        }

        public void LoadText(TextAsset textAsset)
        {
            file = textAsset;
            
            // We assume first page is blank
            var newFile = TextUtilities.RemoveCommentLines(textAsset.text);
            
            pages = newFile.Split('#').ToList();
            pages.RemoveAt(0);
            
            text.text = pages[currentPage];
        }

        private void Update()
        {
            if (closed)
            {
                return;
            }
            
            nextPageObject.SetActive(currentPage + 1 < pages.Count);
            
            touchDelayCurrent -= Time.deltaTime;
            
            if (touchDelayCurrent > 0)
            {
                return;
            }
            
            if (nextPage.WasReleasedThisFrame() && nextPageObject.activeSelf)
            {
                NextPage();
            }

            if (closeAction.WasReleasedThisFrame() || (nextPage.WasReleasedThisFrame() && !nextPageObject.activeSelf))
            {
                CloseWindow();
            }
        }

        public void OnNextPagePressed()
        {
            if (nextPageObject.activeSelf)
            {
                NextPage();
            } else {
                CloseWindow();
            }
        }

        private void NextPage()
        {
            currentPage++;
            text.text = pages[currentPage];
        }

        private void CloseWindow()
        {
            closed = true;
            onClose?.Invoke();
        }
    }
}
