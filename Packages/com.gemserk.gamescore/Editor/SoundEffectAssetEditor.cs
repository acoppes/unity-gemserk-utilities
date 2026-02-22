using System.Collections;
using Game.Components;
using Gemserk.Utilities;
using MyBox;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(SoundEffectAsset))]
    public class SoundEffectAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var soundAsset = target as SoundEffectAsset;
            
            if (soundAsset && GUILayout.Button("Play"))
            {
                var tempObject = new GameObject("Temp-SoundObject");
                tempObject.SetActive(false);
            
                var source = tempObject.AddComponent<AudioSource>();
                source.clip = soundAsset.clips.Random();
                source.pitch = soundAsset.randomPitch.RandomInRange();
                source.volume = soundAsset.volume;
            
                source.gameObject.hideFlags |= HideFlags.DontSave;
                source.playOnAwake = false;
                source.gameObject.SetActive(true);
                source.enabled = true;
                source.Play();

                EditorCoroutineUtility.StartCoroutineOwnerless(DestroySfxOnEnd(source.clip.length, source.gameObject));
                // Destroy(tempObject, source.clip.length);
            }
        }

        public IEnumerator DestroySfxOnEnd(float duration, GameObject sourceObject)
        {
            yield return new WaitForSecondsRealtime(duration);
            DestroyImmediate(sourceObject);
        }
    }
}