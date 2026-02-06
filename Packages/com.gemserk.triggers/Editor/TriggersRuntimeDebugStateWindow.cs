using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities;
using Gemserk.Utilities.Editor;
using MyBox.EditorTools;
using UnityEditor;
using UnityEngine;
using SearchField = UnityEditor.IMGUI.Controls.SearchField;

namespace Gemserk.Triggers.Editor
{
    public class TriggersRuntimeDebugStateWindow : EditorWindow, IHasCustomMenu
    {
        public static bool DisableAutoRenderOnUpdate = false;
        public static bool DisableMultiTriggersRoot = true;
        
        // private const string TriggersRuntimeDebugHideInactiveTriggers = "TriggersRuntimeDebug.HideInactiveTriggers";

        [MenuItem("Window/Gemserk/Triggers/Debug State")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<TriggersRuntimeDebugStateWindow>();
            wnd.titleContent = new GUIContent("Triggers - Debug");
        }

        public class TriggerSystemFoldout
        {
            public string name => !triggerSystem ? null : triggerSystem.name;
            public bool visible => triggerSystem && triggerSystem.isActiveAndEnabled;
            public bool foldout;
            public TriggerSystem triggerSystem;
        }

        public class TriggerFoldout
        {
            public bool foldout;
            public bool expanded;
        }

        private TriggerSystemFoldout[] triggersSystemList;

        private IDictionary<int, TriggerFoldout> foldoutsPerTrigger = new Dictionary<int, TriggerFoldout>();
        
        private int triggerSystemsCount;
        
        private SearchField searchField;
        private Vector2 scroll;
        
        private GUIContent editGuiContent;
        private GUIContent executeGuiContent;
        private GUIContent forceExecuteGuiContent;
        private GUIContent expandGuiContent;
        
        private GUIContent triggerOnGuiContent;
        private GUIContent triggerOffGuiContent;

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
            
            executeGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_PlayButton").image)
            {
                tooltip = "Queue Execution"
            };
            
            forceExecuteGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_StepButton").image)
            {
                tooltip = "Force Execution"
            };
            
            editGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_editicon.sml").image)
            {
                tooltip = "Select"
            };
            expandGuiContent = new GUIContent(EditorGUIUtility.IconContent("FolderOpened On Icon").image)
            {
                tooltip = "Toggle Expand"
            };
            
            triggerOnGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_greenLight").image)
            {
                tooltip = "Active"
            };
            
            triggerOffGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_redLight").image)
            {
                tooltip = "Inactive"
            };
        }

        private void ReloadTriggers()
        {
            CreateInternalList();
            
            var newList = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID).ToList();
            triggerSystemsCount = 0;

            var triggers = new List<TriggerObject>();
            
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
                    
                    triggers.AddRange(triggersSystem.GetComponentsInChildren<TriggerObject>());
                }
            }

            triggerSystemsCount = triggersSystemList.Count(t => t.visible);
            
            foreach (var trigger in triggers)
            {
                if (!foldoutsPerTrigger.ContainsKey(trigger.gameObject.GetInstanceID()))
                {
                    foldoutsPerTrigger[trigger.gameObject.GetInstanceID()] = new TriggerFoldout();
                }
            }
        }
        
        private string searchText;

        private void Update()
        {
            if (DisableAutoRenderOnUpdate)
                return;
            
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

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

            var multipleTriggersRoot = triggerSystemsCount > 1 && !DisableMultiTriggersRoot;

            string[] searchTexts = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTexts = StringUtilities.SplitSearchText(searchText);
            }

            // var hideInactiveTriggers = SessionState.GetBool(TriggersRuntimeDebugHideInactiveTriggers, true);
            // EditorGUI.BeginChangeCheck();
            // hideInactiveTriggers = EditorGUILayout.Toggle("Hide Inactive Triggers", hideInactiveTriggers);
            // if (EditorGUI.EndChangeCheck())
            // {
            //     SessionState.SetBool(TriggersRuntimeDebugHideInactiveTriggers, hideInactiveTriggers);
            // }
            
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
                    
                    if (multipleTriggersRoot)
                        EditorGUI.indentLevel++;
                    
                    foreach (var triggerObject in triggerObjects)
                    {
                        if (searchTexts != null)
                        {
                            if (!StringUtilities.MatchAll(triggerObject.name, searchTexts))
                            {
                                continue;
                            }
                        }
                        
                        // if (hideInactiveTriggers && !triggerObject.isActiveAndEnabled)
                        // {
                        //     continue;
                        // }
                        
                        RenderTriggerObject(triggerObject, actionsDisabled);

                        EditorGUILayout.Separator();
                    }
                    
                    if (multipleTriggersRoot)
                        EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void RenderTriggerObject(TriggerObject triggerObject, bool actionsDisabled)
        {
            var instanceID = triggerObject.gameObject.GetInstanceID();
            var trigger = triggerObject.trigger;

            // var bgStyle = new GUIStyle();
            // bgStyle.normal.background = EditorStyles.label.hover.background;
            
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            
            if (!foldoutsPerTrigger.ContainsKey(instanceID))
            {
                foldoutsPerTrigger[instanceID] = new TriggerFoldout();
            }
                        
            var foldoutStyle = new GUIStyle(EditorStyles.foldout);
            // var foldoutStyle = new GUIStyle(EditorStyles.toolbarButton);
            // var foldoutStyle = new GUIStyle(EditorStyles.objectField);

            var triggerDisabled = triggerObject.IsDisabled();
                        
            if (triggerDisabled)
            {
                foldoutStyle.normal.textColor = Color.gray;
            }
            else
            {
                if (triggerObject.trigger.executionTimes > 0)
                {
                    foldoutStyle.fontStyle = FontStyle.Bold;
                    foldoutStyle.normal.textColor = Color.forestGreen;
                } else if (triggerObject.State == ITrigger.ExecutionState.Executing)
                {
                    foldoutStyle.fontStyle = FontStyle.Bold;
                    foldoutStyle.normal.textColor = Color.yellowNice;
                }
            }
            
            foldoutsPerTrigger[instanceID].foldout =
                EditorGUILayout.Foldout(foldoutsPerTrigger[instanceID].foldout, triggerObject.name, foldoutStyle);
                        
            EditorGUI.BeginDisabledGroup(actionsDisabled || triggerDisabled);
            if (GUILayout.Button(executeGuiContent, GUILayout.MaxWidth(30),
                    GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
            {
                trigger.QueueExecution();
            }
                        
            if (GUILayout.Button(forceExecuteGuiContent, GUILayout.MaxWidth(30),
                    GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
            {
                trigger.ForceQueueExecution();
            }
            EditorGUI.EndDisabledGroup();
                        
            EditorGUI.BeginDisabledGroup(actionsDisabled);
            if (!triggerObject.IsDisabled())
            {
                if (GUILayout.Button(triggerOnGuiContent, GUILayout.MaxWidth(30), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
                {
                    triggerObject.gameObject.SetActive(false);
                }
            }
            else
            {
                if (GUILayout.Button(triggerOffGuiContent, GUILayout.MaxWidth(30), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
                {
                    triggerObject.gameObject.SetActive(true);
                }
            }
            EditorGUI.EndDisabledGroup();
                        
            if (GUILayout.Button(editGuiContent, GUILayout.MaxWidth(30), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
            {
                Selection.activeGameObject = triggerObject.gameObject;
            }
                        
            if (GUILayout.Button(expandGuiContent, GUILayout.MaxWidth(30), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
            {
                foldoutsPerTrigger[instanceID].expanded = !foldoutsPerTrigger[instanceID].expanded;
                MyEditor.FoldInHierarchy(triggerObject.gameObject, foldoutsPerTrigger[instanceID].expanded);
            }
                        
            EditorGUILayout.EndHorizontal();
            
            if (foldoutsPerTrigger[instanceID].foldout)
            {
               
                
                EditorGUI.indentLevel++;
                            
               
                // EditorGUILayout.LabelField("State", trigger.State.ToString());
                        
                // EditorGUILayout.IntField("Pending", trigger.pendingExecutions.Count);
                        
                // if (!triggerDisabled)
                // {
                //     EditorGUILayout.LabelField($"STATUS: {trigger.State.ToString().ToUpper()}");
                // }
                // else
                // {
                //     EditorGUILayout.LabelField("STATUS: INACTIVE"); 
                // }

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
                            
                EditorGUI.BeginDisabledGroup(true);
                if (trigger.actions.Count > 0 && trigger.State == ITrigger.ExecutionState.Executing)
                {
                    var actionObject = trigger.actions[trigger.executingAction] as MonoBehaviour;
                    EditorGUILayout.ObjectField("Current Action",
                        actionObject.gameObject, typeof(GameObject), true);
                }
                else
                {
                    EditorGUILayout.ObjectField("Current Action", null, typeof(GameObject), true);
                }

                EditorGUI.EndDisabledGroup();
                            
                EditorGUI.indentLevel--;
                
            }

            EditorGUILayout.EndVertical();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            EditorWindowExtensions.AddEditScript(menu, nameof(TriggersRuntimeDebugStateWindow));
        }
    }
}