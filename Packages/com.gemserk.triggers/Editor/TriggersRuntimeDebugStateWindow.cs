using System.Linq;
using Gemserk.Utilities;
using UnityEditor;
using UnityEngine;
using SearchField = UnityEditor.IMGUI.Controls.SearchField;

namespace Gemserk.Triggers.Editor
{
    public class TriggersRuntimeDebugStateWindow : EditorWindow
    {
        [MenuItem("Window/Gemserk/Triggers/Debug State")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<TriggersRuntimeDebugStateWindow>();
            wnd.titleContent = new GUIContent("Triggers - Debug");
        }

        public class TriggerSystemFoldout
        {
            public string name => triggerSystem == null ? null : triggerSystem.name;
            public bool visible => triggerSystem != null && triggerSystem.isActiveAndEnabled;
            public bool foldout;
            public TriggerSystem triggerSystem;
        }

        private TriggerSystemFoldout[] triggersSystemList;
        private int triggerSystemsCount;
        
        private SearchField searchField;

        private void OnEnable()
        {
            CreateInternalList();
        }

        private void CreateInternalList()
        {
            if (triggersSystemList == null)
            {
                triggersSystemList = new TriggerSystemFoldout[10];
                for (var i = 0; i < triggersSystemList.Length; i++)
                {
                    triggersSystemList[i] = new TriggerSystemFoldout()
                    {
                        foldout = true,
                        triggerSystem = null
                    };
                }
            }
        }
        
        private void OnHierarchyChange()
        {
            if (!Application.isPlaying)
            {
                ReloadTriggers();
                Repaint();
            }
        }

        private void OnFocus()
        {
            ReloadTriggers();
        }

        private void ReloadTriggers()
        {
            CreateInternalList();
            
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
        
        private string searchText;

        private void OnGUI()
        {
            // if (!Application.isPlaying)
            // {
            //     EditorGUILayout.LabelField("Only available while running");
            //     triggerSystems.Clear();
            // }
            
            if (searchField == null)
            {
                searchField = new SearchField();
            }

            var rect = EditorGUILayout.GetControlRect();
            searchText = searchField.OnGUI(rect, searchText);
            
            var actionsDisabled = !Application.isPlaying;

            var multipleTriggersRoot = triggerSystemsCount > 1;

            string[] searchTexts = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTexts = StringUtilities.SplitSearchText(searchText);
            }

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
                        if (searchTexts != null)
                        {
                            if (!StringUtilities.MatchAll(triggerObject.name, searchTexts))
                            {
                                continue;
                            }
                        }
                        
                        var trigger = triggerObject.trigger;

                        EditorGUI.indentLevel++;
                        
                        EditorGUILayout.BeginVertical();
                        
                        if (triggerObject.isActiveAndEnabled)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(triggerObject.name);
                            EditorGUILayout.LabelField(trigger.State.ToString());
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(triggerObject.name);
                            EditorGUILayout.LabelField("INACTIVE");
                            EditorGUILayout.EndHorizontal();
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