using System.Collections.Generic;
using System.Linq;
using Gemserk.Triggers;
using MyBox.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggersRuntimeDebugStateWindow : EditorWindow
{
    public class TriggerElement
    {
        public TriggerObject triggerObject;
        
        public VisualElement root;
        
        public Button buttonExecute;
        public Button buttonForceExecute;
        public Button buttonExpand;
        public Button buttonState;

        public Label label;

        private bool expanded;

        public TriggerElement(TriggerObject triggerObject)
        {
            this.triggerObject = triggerObject;
        }

        public void SetRootElement(VisualElement rootElement)
        {
            root = rootElement;
            
            label = rootElement.Q<Label>();

            var isTriggerDisabled = triggerObject.IsDisabled();
            
            label.text = triggerObject.name;

            if (isTriggerDisabled)
            {
                label.text = $"{triggerObject.name} [INACTIVE]";
            }
            
            rootElement.RegisterCallback<PointerDownEvent>(evt =>
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
            });
            
            buttonExecute = rootElement.Q<Button>("ButtonExecute");
            buttonExecute.clicked += () =>
            {
                triggerObject.QueueExecution();
            };
            buttonExecute.Q<Image>().image = executeGuiContent.image;
                
            buttonForceExecute = rootElement.Q<Button>("ButtonForceExecute");
            buttonForceExecute.clicked += () =>
            {
                triggerObject.ForceQueueExecution();
            };
            buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
                
            buttonExpand = rootElement.Q<Button>("ButtonExpand");
            buttonExpand.clicked += () =>
            {
                Selection.activeGameObject = triggerObject.gameObject;
                expanded = !expanded;
                MyEditor.FoldInHierarchy(triggerObject.gameObject, expanded);
            };
            buttonExpand.Q<Image>().image = expandGuiContent.image; 
                
            buttonState = rootElement.Q<Button>("ButtonState");
            buttonState.clicked += () =>
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
            };
            
            if (isTriggerDisabled)
            {
                label.AddToClassList("trigger-disabled");
            }
        }

        public void Redraw()
        {
            label.text = triggerObject.name;

            var isDisabled = triggerObject.IsDisabled();
            
            buttonExecute.SetEnabled(Application.isPlaying && !isDisabled);
            buttonForceExecute.SetEnabled(Application.isPlaying && !isDisabled);
            
            label.RemoveFromClassList("trigger-disabled");
            label.RemoveFromClassList("trigger-state-running");
            label.RemoveFromClassList("trigger-state-completed");
            label.RemoveFromClassList("trigger-state-done");
            
            if (isDisabled)
            {
                label.text = $"{triggerObject.name} [INACTIVE:{triggerObject.trigger.executionTimes}]";
                label.AddToClassList("trigger-disabled");
            }
            else
            {
                if (triggerObject.State == ITrigger.ExecutionState.Executing)
                {
                    label.text = $"{triggerObject.name} [RUNNING]";
                    label.AddToClassList("trigger-state-running");
                }
                else
                {
                    if (triggerObject.trigger.maxExecutionTimes > 0 && triggerObject.trigger.executionTimes >= triggerObject.trigger.maxExecutionTimes)
                    {
                        label.text = $"{triggerObject.name} [DONE:{triggerObject.trigger.executionTimes}]";
                        label.AddToClassList("trigger-state-done");
                    }
                    else if (triggerObject.trigger.executionTimes > 0)
                    {
                        label.text = $"{triggerObject.name} [COMPLETED:{triggerObject.trigger.executionTimes}]";
                        label.AddToClassList("trigger-state-completed");
                    }
                }
            }
            
            buttonState.Q<Image>().image = isDisabled  ? triggerOffGuiContent.image : triggerOnGuiContent.image;
        }
    }
    
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    [SerializeField]
    private VisualTreeAsset m_VisualElementTemplate = default;
    
    private static GUIContent executeGuiContent;
    private static GUIContent forceExecuteGuiContent;
    private static GUIContent expandGuiContent;
        
    private static GUIContent triggerOnGuiContent;
    private static GUIContent triggerOffGuiContent;

    // private Dictionary<int, VisualElement> triggerElements = new Dictionary<int, VisualElement>();

    private List<TriggerElement> triggerElements = new List<TriggerElement>();
    private List<TriggerObject> triggerObjects = new List<TriggerObject>();
    
    private VisualElement elementsContainer;
    
    [MenuItem("Window/Gemserk/Triggers/Debug State")]
    public static void ShowWindow()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<TriggersRuntimeDebugStateWindow>();
        wnd.titleContent = new GUIContent("Triggers - Debug");
    }

    private void Update()
    {
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
            
            if (!triggerElement.triggerObject)
            {
                if (triggerElement.root.parent == elementsContainer)
                {
                    elementsContainer.Remove(triggerElement.root);
                }
            }
            else
            {
                triggerElement.Redraw();
            }
        }

        triggerElements.RemoveAll(t => !t.triggerObject);
    }

    private void OnHierarchyChange()
    {
        // check for triggers
        
        triggerObjects = new List<TriggerObject>();
        
        var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        triggerSystems.Select(t => t.GetComponentsInChildren<TriggerObject>(true)).ToList().ForEach(l => triggerObjects.AddRange(l));
        
        for (var i = 0; i < triggerObjects.Count; i++)
        {
            var triggerObject = triggerObjects[i];

            if (triggerObject)
            {
                if (triggerElements.Count(t => t.triggerObject == triggerObject) == 0)
                {
                    var triggerElement = CreateTriggerElement(triggerObject);
                    triggerElements.Add(triggerElement);
                    elementsContainer.Add(triggerElement.root);
                }
            }

            // if (!triggerElements.ContainsKey(triggerObject.gameObject.GetInstanceID()))
            // {
            //     var triggerElement = CreateTriggerElement(triggerObject);
            //     elementsContainer.Add(triggerElement);
            //
            //     triggerElements[triggerObject.gameObject.GetInstanceID()] = triggerElement;
            // }
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);
        
        // Instantiate UXML
        var fromXml = m_VisualTreeAsset.Instantiate();
        // root.Add(fromXml);
        // return;
        
        // var triggerElementTemplate = fromXml.Query<VisualElement>("TriggerElement-Template").First();
        
        elementsContainer = fromXml.Query<VisualElement>("MainScroll").First();
        root.Add(fromXml);

        var button = fromXml.Query<Button>("ButtonRefresh").First();
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
        var triggerObjects = new List<TriggerObject>();
        
        var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        triggerSystems.Select(t => t.GetComponentsInChildren<TriggerObject>(true)).ToList().ForEach(l => triggerObjects.AddRange(l));
        
        // var triggerObjects = FindObjectsByType<TriggerObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        
        for (var i = 0; i < triggerObjects.Count; i++)
        {
            var triggerElement = CreateTriggerElement(triggerObjects[i]);
            
            // if (i % 2 == 0)
            // {
            //     triggerElement.root.AddToClassList("trigger-even");
            // }

            triggerElements.Add(triggerElement);
            elementsContainer.Add(triggerElement.root);
            
            // triggerElements[triggerObjects[i].gameObject.GetInstanceID()] = triggerElement;
        }
        
        // var custom1 = m_VisualElementTemplate.Instantiate();
        // custom1.Q<Label>().text = "MY CUSTOM TEXT 1";
        // mainScroll.Add(custom1);
        //
        // var custom2 = m_VisualElementTemplate.Instantiate();
        //
        // custom2.Q<Label>().text = "MY CUSTOM TEXT 2";
        // mainScroll.Add(custom2);
    }

    private TriggerElement CreateTriggerElement(TriggerObject triggerObject)
    {
        var elementTemplate = m_VisualElementTemplate.Instantiate();

        // var isTriggerDisabled = !triggerObject.gameObject.activeSelf;
        //
        var element = elementTemplate.Q("TriggerElement");
        // var label = element.Q<Label>();
        //
        // label.text = triggerObject.name;
        //
        // if (isTriggerDisabled)
        // {
        //     label.text = $"{triggerObject.name} [INACTIVE]";
        // }
        //
        // element.RegisterCallback<ClickEvent>(evt =>
        // {
        //     EditorGUIUtility.PingObject(triggerObject.gameObject);
        // });
        //
        // var buttonExecute = element.Q<Button>("ButtonExecute");
        // buttonExecute.SetEnabled(Application.isPlaying);
        // buttonExecute.clicked += () =>
        // {
        //     triggerObject.QueueExecution();
        //     // Debug.Log("EXECUTE");
        // };
        // buttonExecute.Q<Image>().image = executeGuiContent.image;
        //     
        // var buttonForceExecute = element.Q<Button>("ButtonForceExecute");
        // buttonForceExecute.SetEnabled(Application.isPlaying);
        // buttonForceExecute.clicked += () =>
        // {
        //     triggerObject.ForceQueueExecution();
        //     // Debug.Log("FORCE EXECUTE");
        // };
        // buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
        //     
        // var buttonExpand = element.Q<Button>("ButtonExpand");
        // buttonExpand.clicked += () =>
        // {
        //     Selection.activeGameObject = triggerObject.gameObject;
        //     // foldoutsPerTrigger[instanceID].expanded = !foldoutsPerTrigger[instanceID].expanded;
        //     MyEditor.FoldInHierarchy(triggerObject.gameObject, true);
        //     // Debug.Log("EXPAND ON/OFF");
        // };
        // buttonExpand.Q<Image>().image = expandGuiContent.image; 
        //     
        // var buttonState = element.Q<Button>("ButtonState");
        // buttonState.clicked += () =>
        // {
        //     if (triggerObject.gameObject.activeSelf)
        //     {
        //         buttonState.Q<Image>().image = triggerOffGuiContent.image;
        //         Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
        //         triggerObject.gameObject.SetActive(false);
        //         EditorUtility.SetDirty(triggerObject.gameObject);
        //         
        //         label.AddToClassList("trigger-disabled");
        //     }
        //     else
        //     {
        //         buttonState.Q<Image>().image = triggerOnGuiContent.image;
        //         Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
        //         triggerObject.gameObject.SetActive(true);
        //         EditorUtility.SetDirty(triggerObject.gameObject);
        //         
        //         label.RemoveFromClassList("trigger-disabled");
        //     }
        // };
        //
        // buttonState.Q<Image>().image = isTriggerDisabled  ? triggerOffGuiContent.image : triggerOnGuiContent.image;
        //
        // if (isTriggerDisabled)
        // {
        //     label.AddToClassList("trigger-disabled");
        // }

        var triggerElement = new TriggerElement(triggerObject);
        triggerElement.SetRootElement(element);

        // triggerElements.Add(triggerElement);

        return triggerElement;
    }
}
