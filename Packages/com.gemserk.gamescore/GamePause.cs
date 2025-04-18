﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    public static class GamePauseResetInEditor
    {
        static GamePauseResetInEditor()
        {
            // reset pauseCount on play/stop in editor
            UnityEditor.EditorApplication.playModeStateChanged += delegate (PlayModeStateChange playModeStateChange)
            {
                if (playModeStateChange == PlayModeStateChange.ExitingEditMode || playModeStateChange == PlayModeStateChange.EnteredEditMode)
                {
                    GamePause.Reset();
                }
            };
        }
    }
    #endif
    

    public static class GamePause
    {
        private static int pausedCount = 0;

        public static bool isPaused => pausedCount > 0;

        private static float previousTimeScale;

        public static event Action onGamePauseChanged;

        public static void Reset()
        {
            pausedCount = 0;
            onGamePauseChanged = null;
        }

        public static void Pause()
        {
            Pause(true);
        }

        public static void Resume()
        {
            Pause(false);
        }
        
        public static void Pause(bool pause)
        {
            if (pause)
            {
                pausedCount++;
                
                if (pausedCount == 1)
                {
                    previousTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                    onGamePauseChanged?.Invoke();
                }
            }
            else
            {
                pausedCount--;

                if (pausedCount == 0)
                {
                    Time.timeScale = previousTimeScale;
                    previousTimeScale = 0;

                    onGamePauseChanged?.Invoke();
                }
            }
            
            Assert.IsTrue(pausedCount >= 0);
        }
    }
}