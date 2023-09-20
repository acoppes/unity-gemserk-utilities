using System;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DebugTools
{
    public class DebugBuffer : MonoBehaviour
    {
        [SerializeField]
        private GameObject actionPrefab;

        [SerializeField]
        private Color historyColor;

        [SerializeField]
        private List<Sprite> actionSprites = new ();
        
        private readonly List<Image> inputActionImages = new ();

        private bool isHistory;
        
        public void Start()
        {
            for (var i = 0; i < BufferedInputComponent.MaxBufferCount; i++)
            {
                var actionInstance = GameObject.Instantiate(actionPrefab, transform);
                actionInstance.SetActive(false);
                inputActionImages.Add(actionInstance.GetComponent<Image>());
            }
        }

        public void ConvertToHistory()
        {
            foreach (var inputAction in inputActionImages)
            {
                inputAction.color = historyColor;
            }

            isHistory = true;
        }

        public void UpdateBuffer(BufferedInputComponent bufferedInput)
        {
            if (isHistory)
            {
                return;
            }

            for (var i = 0; i < inputActionImages.Count; i++)
            {
                var inputActionImage = inputActionImages[i];
                
                inputActionImage.gameObject.SetActive(false);
                
                if (i >= bufferedInput.buffer.Count)
                {
                    continue;
                }

                inputActionImage.gameObject.SetActive(true);
                inputActionImage.sprite = 
                    actionSprites.Find(s => s.name.Equals(bufferedInput.buffer[i], StringComparison.OrdinalIgnoreCase));

                if (inputActionImage.sprite == null)
                {
                    inputActionImage.gameObject.SetActive(false);
                }
            }
        }
    }
}