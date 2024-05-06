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
        private Vector2 scroll;

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
            
            var newList = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
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

            scroll = EditorGUILayout.BeginScrollView(scroll);
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
                        
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.ObjectField(triggerObject.gameObject,  typeof(GameObject), true);
                        EditorGUI.EndDisabledGroup();
                        
                        EditorGUILayout.BeginHorizontal();
                        // EditorGUILayout.LabelField("State", trigger.State.ToString());
                        
                        // EditorGUILayout.IntField("Pending", trigger.pendingExecutions.Count);
                        
                        if (triggerObject.isActiveAndEnabled)
                        {
                            EditorGUILayout.LabelField($"STATUS: {trigger.State.ToString().ToUpper()}");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("STATUS: INACTIVE");
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        
                        EditorGUILayout.LabelField($"Pending: {trigger.pendingExecutions.Count}");
                        
                        if (triggerObject.maxExecutions > 0)
                        {
                            EditorGUILayout.LabelField($"Completed: {trigger.executionTimes}/{triggerObject.maxExecutions}");
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Completed: {trigger.executionTimes}");
                            // EditorGUILayout.IntField("Completed", trigger.executionTimes);
                        }
                        
                        
                        EditorGUILayout.EndHorizontal();

                        if (Application.isPlaying)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            if (trigger.actions.Count > 0 && trigger.State == ITrigger.ExecutionState.Executing)
                            {
                                var actionObject = trigger.actions[trigger.executingAction] as MonoBehaviour;
                                EditorGUILayout.ObjectField("Current Action",
                                    actionObject.gameObject, typeof(GameObject), true);
                            }
                            else
                            {
                                EditorGUILayout.ObjectField("Current Action",
                                    null, typeof(GameObject), true);
                            }

                            EditorGUI.EndDisabledGroup();
                        }
                        
                        if (!actionsDisabled)
                        {
                            EditorGUILayout.BeginHorizontal();
                            
                            if (GUILayout.Button(new GUIContent("Queue", "Queue execution, checks conditions.")))
                            {
                                trigger.QueueExecution();
                            }

                            if (GUILayout.Button(new GUIContent("Force Execution", "Ignores conditions and force object activation.")))
                            {
                                if (!triggerObject.isActiveAndEnabled)
                                {
                                    triggerObject.gameObject.SetActive(true);
                                }
                                
                                trigger.ForceQueueExecution();
                            }
                            
                            if (triggerObject.isActiveAndEnabled)
                            {
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
                        EditorGUILayout.Separator();
                    }
                 
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}