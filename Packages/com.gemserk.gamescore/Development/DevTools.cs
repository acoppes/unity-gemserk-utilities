using System;
using System.Collections;
using System.Collections.Generic;
using Game.Systems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.Development
{
    [Serializable]
    public class DevToolInputCallback
    {
        public InputAction inputAction;
        public UnityEvent unityEvent;
    }
    
    public class DevTools : MonoBehaviour
    {
        public InputAction slowerTimeScale;
        public InputAction fasterTimeScale;
        
        public InputAction toggleHitBoxes;

        public InputAction toggleInputBufferDebug;
        public InputAction toggleGridDebug;
        
        public Text debugTimeScale;

        public GameObject debugInputBufferObject;
        public GameObject gridObject;

        public List<DevToolInputCallback> devActions;

        private Coroutine showTextCoroutine;

        private void Start()
        {
            debugTimeScale.enabled = false;
        }

        private void OnEnable()
        {
            slowerTimeScale.Enable();
            fasterTimeScale.Enable();
            toggleHitBoxes.Enable();
            toggleInputBufferDebug.Enable();
            toggleGridDebug.Enable();

            foreach (var devAction in devActions)
            {
                devAction.inputAction.Enable();
            }
        }
        
        private void OnDisable()
        {
            slowerTimeScale.Disable();
            fasterTimeScale.Disable();
            toggleHitBoxes.Disable();
            toggleInputBufferDebug.Disable();
            toggleGridDebug.Disable();

            foreach (var devAction in devActions)
            {
                devAction.inputAction.Disable();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (toggleInputBufferDebug.WasReleasedThisFrame())
            {
                if (debugInputBufferObject)
                {
                    debugInputBufferObject.SetActive(!debugInputBufferObject.activeInHierarchy);
                }
            }
            
            if (toggleGridDebug.WasReleasedThisFrame())
            {
                if (gridObject)
                {
                    gridObject.SetActive(!gridObject.activeInHierarchy);
                }
            }
            
            if (toggleHitBoxes.WasReleasedThisFrame())
            {
                var debugHitBoxSystem = FindAnyObjectByType<DebugHitBoxSystem>();
                if (debugHitBoxSystem)
                {
                    debugHitBoxSystem.debugHitBoxesEnabled = !debugHitBoxSystem.debugHitBoxesEnabled;
                }
            }
            
            var previousTimeScale = Time.timeScale;
            
            if (slowerTimeScale.WasReleasedThisFrame())
            {
                if (Time.timeScale <= 1.5f && Time.timeScale >= 0.0f)
                {
                    if (Time.timeScale <= 0.1f)
                    {
                        Time.timeScale = 0;
                    } else
                    {
                        Time.timeScale -= 0.1f;
                    }
                } else if (Time.timeScale > 2.0f)
                {
                    Time.timeScale -= 1.0f;
                }
                else if (Time.timeScale > 1.5f)
                {
                    Time.timeScale -= 0.5f;
                }
            }

            if (fasterTimeScale.WasReleasedThisFrame())
            {
                if (Time.timeScale < 1)
                {
                    Time.timeScale += 0.1f;
                } else if (Time.timeScale >= 5.0f)
                {
                    Time.timeScale += 1.0f;
                }
                else if (Time.timeScale >= 1.0f)
                {
                    Time.timeScale += 0.5f;
                }
            }

            if (Time.timeScale < 0)
            {
                Time.timeScale = 0;
            }

            if (Math.Abs(previousTimeScale - Time.timeScale) > Mathf.Epsilon)
            {
                // update
                showTextCoroutine = StartCoroutine(ShowNewTimeScale());
            }
            
            foreach (var devAction in devActions)
            {
                if (devAction.inputAction.WasReleasedThisFrame())
                {
                    devAction.unityEvent.Invoke();   
                }
            }
        }

        private IEnumerator ShowNewTimeScale()
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            debugTimeScale.enabled = true;
            debugTimeScale.text = $"{Time.timeScale:0.0}x";
            yield return new WaitForSecondsRealtime(1.0f);

            debugTimeScale.enabled = false;
        }
    }
}
