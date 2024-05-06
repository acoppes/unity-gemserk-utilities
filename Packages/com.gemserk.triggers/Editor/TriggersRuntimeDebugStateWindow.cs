using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    public class TriggersRuntimeDebugStateWindow : EditorWindow
    {
        [MenuItem("Window/Gemserk/Triggers/Runtime Debug State")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<TriggersRuntimeDebugStateWindow>();
            wnd.titleContent = new GUIContent("Triggers - Runtime Debug");
        }

        public class TriggerSystemFoldout
        {
            public string name => triggerSystem == null ? null : triggerSystem.name;
            public bool visible => triggerSystem != null && triggerSystem.isActiveAndEnabled;
            public bool foldout;
            public TriggerSystem triggerSystem;
        }

        private TriggerSystemFoldout[] triggersSystemList = new TriggerSystemFoldout[10];
        private int triggerSystemsCount;

        private void OnEnable()
        {
            for (var i = 0; i < triggersSystemList.Length; i++)
            {
                triggersSystemList[i] = new TriggerSystemFoldout()
                {
                    foldout = true,
                    triggerSystem = null
                };
            }
        }

        private void OnFocus()
        {
            var newList = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            triggerSystemsCount = 0;
            
            for (var i = 0; i < triggersSystemList.Length; i++)
            {
                var triggerSystemFoldout = triggersSystemList[i];
                if (triggerSystemFoldout == null)
                {
                    triggerSystemFoldout = new TriggerSystemFoldout()
                    {
                        foldout = true
                    };
                }
                
                triggerSystemFoldout.triggerSystem = null;
                
                if (i < newList.Count)
                {
                    var triggersSystem = newList[i];
                    triggerSystemFoldout.triggerSystem = triggersSystem;
                }
            }

            triggerSystemsCount = triggersSystemList.Count(t => t.visible);
        }

        private void OnGUI()
        {
            // if (!Application.isPlaying)
            // {
            //     EditorGUILayout.LabelField("Only available while running");
            //     triggerSystems.Clear();
            // }

            var actionsDisabled = !Application.isPlaying;

            var multipleTriggersRoot = triggerSystemsCount > 1;

            foreach (var triggersSystemFoldout in triggersSystemList)
            {
                if (!triggersSystemFoldout.visible)
                {
                    continue;
                }
                
                if (multipleTriggersRoot)
                {
                    triggersSystemFoldout.foldout =
                        EditorGUILayout.Foldout(triggersSystemFoldout.foldout, triggersSystemFoldout.name);
                }
                
                if (!multipleTriggersRoot || triggersSystemFoldout.foldout)
                {
                    var triggerSystem = triggersSystemFoldout.triggerSystem;
                    var triggerObjects = triggerSystem.GetComponentsInChildren<TriggerObject>(true);

                    foreach (var triggerObject in triggerObjects)
                    {
                        var trigger = triggerObject.trigger;

                        EditorGUI.indentLevel++;
                        
                        EditorGUILayout.BeginVertical();
                        
                        if (triggerObject.isActiveAndEnabled)
                        {
                            EditorGUILayout.LabelField(triggerObject.name, trigger.State.ToString());
                        }
                        else
                        {
                            EditorGUILayout.LabelField(triggerObject.name, "INACTIVE");
                        }
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.BeginDisabledGroup(true);
                        // EditorGUILayout.LabelField("State", trigger.State.ToString());
                        EditorGUILayout.IntField("Pending", trigger.pendingExecutions.Count);
                        EditorGUILayout.IntField("Completed", trigger.executionTimes);
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.EndHorizontal();

                        if (!actionsDisabled)
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (triggerObject.isActiveAndEnabled)
                            {
                                if (GUILayout.Button("Queue"))
                                {
                                    trigger.QueueExecution();
                                }

                                if (GUILayout.Button("Force"))
                                {
                                    trigger.ForceQueueExecution();
                                }

                                if (GUILayout.Button("Deactivate"))
                                {
                                    triggerObject.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Activate"))
                                {
                                    triggerObject.gameObject.SetActive(true);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        
                        EditorGUILayout.EndVertical();

                        EditorGUI.indentLevel--;
                    }
                }
            }
        }
    }
}