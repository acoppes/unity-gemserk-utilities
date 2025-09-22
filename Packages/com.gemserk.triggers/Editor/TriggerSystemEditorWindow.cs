using System;
using System.Collections.Generic;
using Gemserk.Utilities;
using Gemserk.Utilities.Editor;
using SandolkakosDigital.EditorUtils;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    public class TriggerSystemEditorWindow : EditorWindow, IHasCustomMenu
    {
        private GameObject selectedGameObject;
        private TriggerObject selectedTrigger;
        private TriggerSystem selectedTriggerSystem;

        private TriggerScope triggerScope;
        private Transform triggerEventsParent;
        private Transform triggerConditionsParent;
        private Transform triggerActionsParent;
        private TriggerSystemFilteredTypeCache triggerTypes;
        
        
        private Vector2 scrollPositionTriggerTypes;
        private Vector2 scrollPositionTriggersList;
        
        private SearchField searchField;
        private string searchText;

        private List<TriggerObject> cachedTriggers = new();

        private enum TriggerScope
        {
            Events, Conditions, Actions, Trigger, TriggerSystem, None
        }

        [MenuItem("Window/Gemserk/Triggers Editor")]
        public static void ShowWindow()
        {
            GetWindow<TriggerSystemEditorWindow>("Triggers Editor");
        }

        private void OnGUI()
        {
            NeedsToRefreshState();
            
            EditorGUILayout.BeginVertical();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Selected GameObject", selectedGameObject, typeof(GameObject), true);
            EditorGUILayout.ObjectField("Selected Trigger", selectedTrigger, typeof(TriggerObject), true);
            EditorGUILayout.ObjectField("Selected TriggerSystem", selectedTriggerSystem, typeof(TriggerSystem), true);
            EditorGUILayout.EnumPopup("TriggerScope", triggerScope);
            EditorGUI.EndDisabledGroup();

            if (triggerScope == TriggerScope.None)
            {
                if (GUILayout.Button("Find Or Create Trigger System"))
                {
                    var triggerSystem = GameObject.FindAnyObjectByType<TriggerSystem>(FindObjectsInactive.Exclude);

                    if (triggerSystem == null)
                    {
                        var triggerSystemObject = new GameObject("~TriggerSystem");
                        triggerSystem = triggerSystemObject.AddComponent<TriggerSystem>();
                        EditorUtility.SetDirty(triggerSystemObject);
                    }
                    
                    Selection.SetActiveObjectWithContext(triggerSystem.gameObject, null);
                    EditorGUIUtility.PingObject(triggerSystem.gameObject);
                    Repaint();
                }

            } else if (triggerScope == TriggerScope.TriggerSystem)
            {
                if (GUILayout.Button("New Trigger"))
                {
                    var triggerObject = new GameObject("Trigger");
                    triggerObject.transform.SetParent(selectedTriggerSystem.transform);
                    var trigger = triggerObject.AddComponent<TriggerObject>();
                    triggerObject.transform.FindOrCreateFolder("Events");
                    triggerObject.transform.FindOrCreateFolder("Conditions");
                    triggerObject.transform.FindOrCreateFolder("Actions");
                    SceneHierarchyUtility.SetExpanded(triggerObject, true);
                    EditorGUIUtility.PingObject(triggerObject);
                    Selection.SetActiveObjectWithContext(triggerObject, null);
                    EditorUtility.SetDirty(triggerObject);
                    Repaint();
                }
                
                EditorGUILayout.Separator();


                cachedTriggers.Clear();
                selectedTriggerSystem.gameObject.GetComponentsInChildrenDepth1(true, true, cachedTriggers);
                scrollPositionTriggersList = EditorGUILayout.BeginScrollView(scrollPositionTriggersList);

                foreach (var trigger in cachedTriggers)
                {
                    if (GUILayout.Button(trigger.name))
                    {
                        SceneHierarchyUtility.SetExpanded(trigger.gameObject, true);
                        EditorGUIUtility.PingObject(trigger.gameObject);
                        Selection.SetActiveObjectWithContext(trigger.gameObject, null);
                        Repaint();
                    }
                }
                
                EditorGUILayout.EndScrollView();
            }
            else
            {
                if (GUILayout.Button("GO TO TRIGGER SYSTEM"))
                {
                    SceneHierarchyUtility.SetExpanded(selectedTriggerSystem.gameObject, true);
                    EditorGUIUtility.PingObject(selectedTriggerSystem.gameObject);
                    Selection.SetActiveObjectWithContext(selectedTriggerSystem.gameObject, null);
                    Repaint();
                }
                
                EditorGUILayout.Separator();
                if (GUILayout.Button($"TRIGGER: {selectedTrigger.name}"))
                {
                    SceneHierarchyUtility.SetExpanded(selectedTrigger.gameObject, true);
                    EditorGUIUtility.PingObject(selectedTrigger.gameObject);
                    Selection.SetActiveObjectWithContext(selectedTrigger.gameObject, null);
                    Repaint();
                }
                
                if (searchField == null)
                {
                    searchField = new SearchField();
                }

                var oldSearchText = searchText;
                var rect = EditorGUILayout.GetControlRect();
                searchText = searchField.OnGUI(rect, searchText);

                if (oldSearchText != searchText)
                {
                    this.triggerTypes.Filter(searchText);
                }

                bool drawConditions = true;
                bool drawEvents = true;
                bool drawActions = true;

                GameObject newItemParent = selectedGameObject;

                switch (triggerScope)
                {
                    case TriggerScope.Actions:
                        drawEvents = false;
                        break;
                    case TriggerScope.Conditions:
                        drawEvents = false;
                        drawActions = false;
                        break;
                    case TriggerScope.Events:
                        drawActions = false;
                        break;
                    case TriggerScope.Trigger:
                        newItemParent = null;
                        break;
                }

                scrollPositionTriggerTypes = EditorGUILayout.BeginScrollView(scrollPositionTriggerTypes);
                
                if (drawEvents)
                {
                    DrawTriggerButtons("Events", triggerTypes.filteredEventTypes, triggerTypes.allEventTypes, newItemParent != null ? newItemParent.transform : triggerEventsParent, triggerEventsParent);
                }

                if (drawConditions)
                {
                    DrawTriggerButtons("Conditions", triggerTypes.filteredConditionTypes,triggerTypes.allConditionTypes,  newItemParent != null ? newItemParent.transform : triggerConditionsParent, triggerConditionsParent);
                }

                if (drawActions)
                {
                    DrawTriggerButtons("Actions", triggerTypes.filteredActionTypes, triggerTypes.allActionTypes, newItemParent != null ? newItemParent.transform : triggerActionsParent, triggerActionsParent);
                }
                
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();
        }

        private bool NeedsToRefreshState()
        {
            bool shouldRefreshState = false;
            switch (triggerScope)
            {
                case TriggerScope.Trigger:
                case TriggerScope.Actions:
                case TriggerScope.Events:
                case TriggerScope.Conditions:
                    if (selectedTrigger == null || triggerEventsParent == null || triggerActionsParent == null || triggerConditionsParent == null)
                    {
                        shouldRefreshState = true;
                    }
                    break;
                case TriggerScope.TriggerSystem:
                    if (selectedTriggerSystem == null)
                    {
                        shouldRefreshState = true;
                    }
                    break;
            }
            
            OnSelectionChange();
            return shouldRefreshState;
        }

        private void OnSelectionChange()
        {
            var activeTransform = Selection.activeTransform;
            if (activeTransform == null)
            {
                selectedGameObject = null;
                selectedTrigger = null;
                selectedTriggerSystem = null;
                triggerActionsParent = null;
                triggerEventsParent = null;
                triggerConditionsParent = null;
                triggerScope = TriggerScope.None;
                return;
            }

            selectedGameObject = activeTransform.gameObject;
            selectedTrigger = selectedGameObject.GetComponentInParent<TriggerObject>(includeInactive:true);
            selectedTriggerSystem = selectedGameObject.GetComponentInParent<TriggerSystem>(includeInactive:true);

            
            if (selectedTriggerSystem != null && selectedGameObject == selectedTriggerSystem.gameObject)
            {
                triggerScope = TriggerScope.TriggerSystem;
            } else if (selectedTrigger != null && selectedGameObject == selectedTrigger.gameObject)
            {
                triggerScope = TriggerScope.Trigger;
            } else if (selectedTrigger == null)
            {
                triggerScope = TriggerScope.None;
            }
            else
            {
                var currentTranform = selectedGameObject.transform;
                var parentTransform = currentTranform.parent;
                while (parentTransform.GetComponent<TriggerObject>() == null)
                {
                    currentTranform = parentTransform;
                    parentTransform = currentTranform.transform.parent;
                }

                triggerScope = currentTranform.name switch
                {
                    "Events" => TriggerScope.Events,
                    "Conditions" => TriggerScope.Conditions,
                    "Actions" => TriggerScope.Actions,
                    _ => TriggerScope.None
                };
            }

            if (selectedTrigger != null)
            {
                triggerEventsParent = selectedTrigger.transform.FindOrCreateFolder("Events");
                triggerConditionsParent = selectedTrigger.transform.FindOrCreateFolder("Conditions");
                triggerActionsParent = selectedTrigger.transform.FindOrCreateFolder("Actions");
            }

            Repaint();
        }

        private void OnEnable()
        {
            this.triggerTypes = new TriggerSystemFilteredTypeCache();
            this.triggerTypes.Init();
            this.triggerTypes.Filter(searchText);
        }

        public void DrawTriggerButtons(string category, List<TriggerSystemFilteredTypeCache.TypeInfo> filteredTypesCollection, List<TriggerSystemFilteredTypeCache.TypeInfo> fullTypesCollection, Transform parent, Transform alternativeParent)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField($"Add {category} - {filteredTypesCollection.Count}/{fullTypesCollection.Count}");
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal();

            float totalWidth = 0f;
            float padding = 5f; // Padding between buttons

            for (int i = 0; i < filteredTypesCollection.Count; i++)
            {
                var typeInfo = filteredTypesCollection[i];
                // Calculate the width of the button dynamically
                float buttonWidth = EditorStyles.miniButton.CalcSize(new GUIContent(typeInfo.visualName)).x + padding;
                //float buttonWidth = 150;

                buttonWidth = Mathf.Max(100, buttonWidth);
                
                // Check if adding the next button would exceed the available width
                if (totalWidth + buttonWidth > (position.width - 10))
                {
                    // End the current horizontal layout group and start a new one
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    totalWidth = 0f; // Reset total width for the new line
                }

                // Create a button with the label

                var buttonContent = new GUIContent(typeInfo.visualName);

                if (!string.IsNullOrEmpty(typeInfo.tooltip))
                {
                    buttonContent.tooltip = typeInfo.tooltip;
                }
                
                if (GUILayout.Button(buttonContent, GUILayout.Width(buttonWidth)))
                {
                    Undo.IncrementCurrentGroup();
                    var newActionObject = new GameObject(typeInfo.visualName);
                    Undo.RegisterCreatedObjectUndo(newActionObject, $"Created {typeInfo.visualName} Trigger Stuff");
                    Undo.AddComponent(newActionObject,typeInfo.type);
                    var parentToUse = Event.current.shift ? parent : alternativeParent;
                    Undo.SetTransformParent(newActionObject.transform, parentToUse, false, "Setting parent for trigger stuff");
                    Undo.RegisterFullObjectHierarchyUndo(newActionObject,$"Created {typeInfo.visualName} Trigger Stuff" );
                    
                    Undo.SetCurrentGroupName($"Created {typeInfo.visualName} Trigger Stuff");
                    Selection.activeObject = newActionObject;
                    EditorGUIUtility.PingObject(newActionObject);
                }

                // Add the width of the current button to the total width
                totalWidth += buttonWidth;
            }

            // End the final horizontal layout group
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            EditorWindowExtensions.AddEditScript(menu, nameof(TriggerSystemEditorWindow));
        }
    }
}