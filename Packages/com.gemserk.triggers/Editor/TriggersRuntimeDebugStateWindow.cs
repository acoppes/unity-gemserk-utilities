using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities.Editor;
using MyBox;
using MyBox.EditorTools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gemserk.Triggers.Editor
{
    public class TriggersRuntimeDebugStateWindow : EditorWindow, IHasCustomMenu
    {
        public class TriggerElement
        {
            public ITrigger trigger;
        
            public VisualElement root;
        
            public Button buttonExecute;
            public Button buttonForceExecute;
            public Button buttonExpand;
            public Button buttonState;

            public Label label;

            private bool expanded;

            private bool wasDisabled;
            private ITrigger.ExecutionState previousState = ITrigger.ExecutionState.Waiting;
            private int previousExecutionTimes;
            private int maxExecutionTimes;

            public TriggerElement(ITrigger trigger)
            {
                this.trigger = trigger;
            }

            public void SetRootElement(VisualElement rootElement)
            {
                root = rootElement;
            
                label = rootElement.Q<Label>();

                var isTriggerDisabled = trigger.IsDisabled();
            
                label.text = trigger.Name;
            
                rootElement.RegisterCallback<PointerDownEvent>(evt =>
                {
                    if (trigger is TriggerObject triggerObject)
                    {
                        if (evt.button == 1)
                        {
                            EditorGUIUtility.PingObject(triggerObject.gameObject);
                        } else if (evt.button == 0)
                        {
                            Selection.activeGameObject = triggerObject.gameObject;
                            expanded = !expanded;
                            MyEditor.FoldInHierarchy(triggerObject.gameObject, expanded);
                        }
                    }

                });
            
                buttonExecute = rootElement.Q<Button>("ButtonExecute");
                buttonExecute.clicked += () =>
                {
                    trigger.QueueExecution();
                };
                buttonExecute.Q<Image>().image = executeGuiContent.image;
                
                buttonForceExecute = rootElement.Q<Button>("ButtonForceExecute");
                buttonForceExecute.clicked += () =>
                {
                    trigger.ForceQueueExecution();
                };
                buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
                
                buttonExpand = rootElement.Q<Button>("ButtonExpand");
                buttonExpand.clicked += () =>
                {
                    if (trigger is TriggerObject triggerObject)
                    {
                        Selection.activeGameObject = triggerObject.gameObject;
                        expanded = !expanded;
                        MyEditor.FoldInHierarchy(triggerObject.gameObject, expanded);
                    }
                };
                buttonExpand.Q<Image>().image = expandGuiContent.image; 
                
                buttonState = rootElement.Q<Button>("ButtonState");
                buttonState.clicked += () =>
                {
                    if (trigger is TriggerObject triggerObject)
                    {
                        if (triggerObject.gameObject.activeSelf)
                        {
                            buttonState.Q<Image>().image = triggerOffGuiContent.image;
                            Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
                            triggerObject.gameObject.SetActive(false);
                            EditorUtility.SetDirty(triggerObject.gameObject);
                    
                            label.AddToClassList("trigger-disabled");
                        }
                        else
                        {
                            buttonState.Q<Image>().image = triggerOnGuiContent.image;
                            Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
                            triggerObject.gameObject.SetActive(true);
                            EditorUtility.SetDirty(triggerObject.gameObject);
                    
                            label.RemoveFromClassList("trigger-disabled");
                        }
                    }
                    

                };
            
                if (isTriggerDisabled)
                {
                    label.AddToClassList("trigger-disabled");
                }
            
                Redraw(true);
            }

            public void Redraw(bool force = false)
            {
                var triggerInstance = trigger as Trigger;
                var parentName = string.Empty;
                var triggerName = string.Empty;
                
                var maxExecutions = 0;

                if (triggerInstance != null)
                {
                    maxExecutions = triggerInstance.maxExecutionTimes;
                    triggerName = triggerInstance.Name;
                }
                
                if (trigger is TriggerObject triggerObject)
                {
                    if (!triggerObject || !triggerObject.gameObject)
                    {
                        return;
                    }
                    
                    triggerInstance = triggerObject.trigger;
                    triggerName = triggerObject.gameObject ? triggerObject.gameObject.name : string.Empty;
                    
                    if (triggerObject.transform.parent)
                    {
                        var triggerSystem = triggerObject.transform.parent.GetComponent<TriggerSystem>();
                        if (!triggerSystem)
                        {
                            parentName = triggerObject.transform.parent.name;
                        }
                    }
                    
                    maxExecutions = triggerObject.GetCalculatedMaxExecutions();
                } 

                if (triggerInstance == null)
                {
                    return;
                }

                var isDisabled = triggerInstance.IsDisabled();
                var currentState = triggerInstance.State;
                
                if (!force)
                {
                    if (wasDisabled == isDisabled && currentState == previousState && previousExecutionTimes == triggerInstance.executionTimes && 
                        maxExecutionTimes == maxExecutions)
                    {
                        return;
                    }
                }
                

                previousState = currentState;
                wasDisabled = isDisabled;
                previousExecutionTimes = triggerInstance.executionTimes;
                maxExecutionTimes = maxExecutions;
            
                // var hidden = (triggerObject.gameObject.activeSelf && !triggerObject.gameObject.activeInHierarchy);
                // root.visible = !hidden;

                var cantExecute = triggerInstance.maxExecutionTimes > 0 &&
                                  triggerInstance.executionTimes >= triggerInstance.maxExecutionTimes;
            
                buttonExecute.SetEnabled(Application.isPlaying && !isDisabled && !cantExecute);
                buttonForceExecute.SetEnabled(Application.isPlaying && !isDisabled);

                label.RemoveFromClassList("trigger-disabled");
                label.RemoveFromClassList("trigger-state-running");
                label.RemoveFromClassList("trigger-state-completed");
                label.RemoveFromClassList("trigger-state-done");

                var executionNumber = $"{triggerInstance.executionTimes}";

                if (triggerInstance.maxExecutionTimes > 0)
                {
                    executionNumber = $"{triggerInstance.executionTimes}/{maxExecutionTimes}";
                }

                var suffix = string.Empty;

                if (isDisabled)
                {
                    suffix = $"[INACTIVE:{executionNumber}]";
                    label.AddToClassList("trigger-disabled");
                }
                else
                {
                    if (currentState == ITrigger.ExecutionState.Executing)
                    {
                        executionNumber = $"{triggerInstance.executionTimes + 1}/{maxExecutionTimes}";
                    
                        suffix = $"[RUNNING:{executionNumber}]";
                        label.AddToClassList("trigger-state-running");
                    }
                    else
                    {
                        if (triggerInstance.executionTimes > 0)
                        {
                            suffix = $"[COMPLETED:{executionNumber}]";
                            label.AddToClassList("trigger-state-completed");
                        }
                        else
                        {
                            if (maxExecutionTimes > 0)
                            {
                                suffix = $"[0/{maxExecutionTimes}]";
                            }
                        }
                    }
                }
            
                var text = string.IsNullOrEmpty(parentName) ? $"{triggerName}" : $"{parentName}/{triggerName}";
                if (!string.IsNullOrEmpty(suffix))
                {
                    text += $" {suffix}";
                }
                
                label.text = text;

                buttonState.Q<Image>().image = isDisabled ? triggerOffGuiContent.image : triggerOnGuiContent.image;
            }
        }
    
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        public StyleSheet styleSheet;
    
        [SerializeField]
        private VisualTreeAsset m_VisualElementTemplate = default;
    
        private static GUIContent executeGuiContent;
        private static GUIContent forceExecuteGuiContent;
        private static GUIContent expandGuiContent;
        
        private static GUIContent triggerOnGuiContent;
        private static GUIContent triggerOffGuiContent;

        // private Dictionary<int, VisualElement> triggerElements = new Dictionary<int, VisualElement>();

        private List<TriggerElement> triggerElements = new List<TriggerElement>();
        private List<ITrigger> triggers = new List<ITrigger>();
    
        private VisualElement elementsContainer;
        private ToolbarSearchField searchToolbar;
        private string searchText;

        [MenuItem("Window/Gemserk/Triggers/Debug State")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<TriggersRuntimeDebugStateWindow>();
            wnd.titleContent = new GUIContent("Triggers - Debug");
        }
    
        private VisualElement CreateSearchToolbar()
        {
            searchToolbar = new ToolbarSearchField();
            searchToolbar.AddToClassList("searchToolbar");
            searchToolbar.RegisterValueChangedCallback(evt =>
            {
                searchText = evt.newValue;
                Redraw(true);
            });

            return searchToolbar;
        }

        private void Update()
        {
            for (var i = 0; i < triggerElements.Count; i++)
            {
                var triggerElement = triggerElements[i];
            
                if (triggerElement.trigger == null || !triggers.Contains(triggerElement.trigger))
                {
                    if (triggerElement.root.parent == elementsContainer)
                    {
                        elementsContainer.Remove(triggerElement.root);
                    }
                }
            }

            triggerElements.RemoveAll(t => t.trigger == null || !triggers.Contains(t.trigger));
        
            Redraw(false);
        }

        private void Redraw(bool forced)
        {
            string[] searchTexts = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchTexts = searchText.Split(' ');
                }
            }
        
            for (var i = 0; i < triggerElements.Count; i++)
            {
                var triggerElement = triggerElements[i];
            
                triggerElement.root.style.display = DisplayStyle.Flex;

                if (triggerElement.trigger != null)
                {
                    if (!searchText.IsNullOrEmpty())
                    {
                        if (searchTexts != null && searchTexts.Length > 0)
                        {
                            var match = true;
                        
                            foreach (var text in searchTexts)
                            {
                                if (!triggerElement.trigger.Name.ToLower().Contains(text.ToLower()))
                                {
                                    match = false;
                                }
                            }

                            if (!match)
                            {
                                triggerElement.root.style.display = DisplayStyle.None;
                                continue;
                            }
                        }
                    }  
                }
            
                triggerElement.Redraw(forced);
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredEditMode)
            {
                if (triggerElements != null)
                {
                    triggerElements.Clear();
                }
                if (elementsContainer != null)
                {
                    elementsContainer.Clear();
                }
                Reload();
            } else if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                if (triggerElements != null)
                {
                    triggerElements.Clear();
                }
                if (elementsContainer != null)
                {
                    elementsContainer.Clear();
                }
                Reload();
            }
            
            Redraw(true);
        }

        private void OnHierarchyChange()
        {
            // check for triggers

            if (Application.isPlaying)
            {
                return;
            }
        
            triggers = new List<ITrigger>();
        
            var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            
            // triggerSystems.Select(t => t.GetComponentsInChildren<TriggerObject>(true)).ToList().ForEach(l => triggers.AddRange(l));
            
            triggerSystems.ForEach(ts =>
            {
                if (ts.triggers != null && ts.triggers.Count > 0)
                {
                    triggers.AddRange(ts.triggers);
                }
                else
                {
                    ts.GetComponentsInChildren(true, triggers);
                }
            });
        
            for (var i = 0; i < triggers.Count; i++)
            {
                var trigger = triggers[i];

                if (triggerElements.Count(t => t.trigger == trigger) == 0)
                {
                    var triggerElement = CreateTriggerElement(trigger);
                    triggerElements.Add(triggerElement);
                    elementsContainer.Add(triggerElement.root);
                }
            }
            
            for (var i = 0; i < triggerElements.Count; i++)
            {
                var triggerElement = triggerElements[i];
        
                if (triggerElement.root != null)
                {
                    if (i % 2 == 0)
                    {
                        triggerElement.root.AddToClassList("trigger-even");
                    }
                    else
                    {
                        triggerElement.root.RemoveFromClassList("trigger-even");
                    }
                }
            }
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
      
            root.Add(CreateSearchToolbar());
            searchText = string.Empty;
        
            var template = m_VisualTreeAsset.Instantiate();
        
            elementsContainer = template.Query<VisualElement>("MainScroll").First();
            root.Add(template);
        
            if (styleSheet)
            {
                root.styleSheets.Add(styleSheet);
            }

            var button = template.Query<Button>("ButtonRefresh").First();
            button.clicked += () =>
            {
                triggerElements.Clear();
                elementsContainer.Clear();
                Reload();
            };
        
            executeGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_PlayButton").image)
            {
                tooltip = "Queue Execution"
            };
        
            forceExecuteGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_StepButton").image)
            {
                tooltip = "Force Execution"
            };
        
            expandGuiContent = new GUIContent(EditorGUIUtility.IconContent("FolderOpened On Icon").image)
            {
                tooltip = "Toggle Expand"
            };
        
            triggerOnGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_greenLight").image)
            {
                tooltip = "Active"
            };
            
            triggerOffGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_lightOff").image)
            {
                tooltip = "Inactive"
            };

            Reload();
        }

        private void Reload()
        {
            triggers = new List<ITrigger>();
        
            var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);

            // triggerSystems.Select(t =>
            // {
            //     t.triggers.ForEach(t =>
            //     {
            //         if (t is Trigger)
            //         {
            //             triggers.Add(t);
            //         }
            //         else if (t is TriggerObject to)
            //         {
            //             triggers.AddRange(to.GetComponentsInChildren<TriggerObject>());
            //         }
            //     });
            //     
            //     return t.GetComponentsInChildren<TriggerObject>(true);
            // }).ToList().ForEach(l => triggers.AddRange(l));

            triggerSystems.ForEach(ts =>
            {
                if (ts.triggers != null && ts.triggers.Count > 0)
                {
                    triggers.AddRange(ts.triggers);
                }
                else
                {
                    ts.GetComponentsInChildren(true, triggers);
                }
            });
            
            // var triggerObjects = FindObjectsByType<TriggerObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        
            for (var i = 0; i < triggers.Count; i++)
            {
                var triggerElement = CreateTriggerElement(triggers[i]);
                triggerElements.Add(triggerElement);
                elementsContainer.Add(triggerElement.root);
            
                if (triggerElement.root != null)
                {
                    if (i % 2 == 0)
                    {
                        triggerElement.root.AddToClassList("trigger-even");
                    }
                    else
                    {
                        triggerElement.root.RemoveFromClassList("trigger-even");
                    }
                }
            }
        }

        private TriggerElement CreateTriggerElement(ITrigger trigger)
        {
            var elementTemplate = m_VisualElementTemplate.Instantiate();

            var triggerElement = new TriggerElement(trigger);
            triggerElement.SetRootElement(elementTemplate.Q("TriggerElement"));

            return triggerElement;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            EditorWindowExtensions.AddEditScript(menu, nameof(TriggersRuntimeDebugStateWindow));
        }
    }
}
