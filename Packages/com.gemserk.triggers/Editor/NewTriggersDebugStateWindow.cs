using System.Collections.Generic;
using System.Linq;
using Gemserk.Triggers;
using MyBox;
using MyBox.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NewTriggersDebugStateWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    [SerializeField]
    private VisualTreeAsset m_VisualElementTemplate = default;
    
    private GUIContent executeGuiContent;
    private GUIContent forceExecuteGuiContent;
    private GUIContent expandGuiContent;
        
    private GUIContent triggerOnGuiContent;
    private GUIContent triggerOffGuiContent;

    [MenuItem("Window/UI Toolkit/NewTriggersDebugStateWindow")]
    public static void ShowExample()
    {
        var wnd = GetWindow<NewTriggersDebugStateWindow>();
        wnd.titleContent = new GUIContent("NewTriggersDebugStateWindow");
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
        
        var mainScroll = fromXml.Query<VisualElement>("MainScroll").First();
        root.Add(mainScroll);

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

        var triggerObjects = new List<TriggerObject>();
        
        var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        triggerSystems.Select(t => t.GetComponentsInChildren<TriggerObject>(true)).ToList().ForEach(l => triggerObjects.AddRange(l));
        
        // var triggerObjects = FindObjectsByType<TriggerObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        
        for (int i = 0; i < triggerObjects.Count; i++)
        {
            // var custom = m_VisualElementTemplate.Instantiate();
            //
            // var element = custom.Q("TriggerElement");
            //
            // if (i % 2 == 0)
            // {
            //     element.AddToClassList("trigger-even");
            // }
            //
            // // custom.Q<Foldout>().value = false;
            //
            // element.Q<Label>().text = "MY CUSTOM TEXT 1";
            // var buttonExecute = element.Q<Button>("ButtonExecute");
            // buttonExecute.clicked += () =>
            // {
            //     Debug.Log("EXECUTE");
            // };
            // buttonExecute.Q<Image>().image = executeGuiContent.image;
            //
            // var buttonForceExecute = element.Q<Button>("ButtonForceExecute");
            // buttonForceExecute.clicked += () =>
            // {
            //     Debug.Log("FORCE EXECUTE");
            // };
            // buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
            //
            // var buttonExpand = element.Q<Button>("ButtonExpand");
            // buttonExpand.SetEnabled(false);
            // buttonExpand.clicked += () =>
            // {
            //     Debug.Log("EXPAND ON/OFF");
            // };
            // buttonExpand.Q<Image>().image = expandGuiContent.image; 
            //
            // var buttonState = element.Q<Button>("ButtonState");
            // buttonState.clicked += () =>
            // {
            //     Debug.Log("STATE ON/OFF");
            // };
            // buttonState.Q<Image>().image = triggerOnGuiContent.image;

            var triggerElement = CreateTriggerElement(triggerObjects[i]);
            
            if (i % 2 == 0)
            {
                triggerElement.Q("TriggerElement").AddToClassList("trigger-even");
            }
            
            mainScroll.Add(triggerElement);
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

    private VisualElement CreateTriggerElement(TriggerObject triggerObject)
    {
        var triggerElement = m_VisualElementTemplate.Instantiate();

        var element = triggerElement.Q("TriggerElement");
        element.Q<Label>().text = triggerObject.name;
        element.RegisterCallback<ClickEvent>(evt =>
        {
            EditorGUIUtility.PingObject(triggerObject.gameObject);
        });
        
        var buttonExecute = element.Q<Button>("ButtonExecute");
        buttonExecute.SetEnabled(Application.isPlaying);
        buttonExecute.clicked += () =>
        {
            triggerObject.QueueExecution();
            // Debug.Log("EXECUTE");
        };
        buttonExecute.Q<Image>().image = executeGuiContent.image;
            
        var buttonForceExecute = element.Q<Button>("ButtonForceExecute");
        buttonForceExecute.SetEnabled(Application.isPlaying);
        buttonForceExecute.clicked += () =>
        {
            triggerObject.ForceQueueExecution();
            // Debug.Log("FORCE EXECUTE");
        };
        buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
            
        var buttonExpand = element.Q<Button>("ButtonExpand");
        buttonExpand.clicked += () =>
        {
            Selection.activeGameObject = triggerObject.gameObject;
            // foldoutsPerTrigger[instanceID].expanded = !foldoutsPerTrigger[instanceID].expanded;
            MyEditor.FoldInHierarchy(triggerObject.gameObject, true);
            // Debug.Log("EXPAND ON/OFF");
        };
        buttonExpand.Q<Image>().image = expandGuiContent.image; 
            
        var buttonState = element.Q<Button>("ButtonState");
        buttonState.clicked += () =>
        {
            if (triggerObject.gameObject.activeSelf)
            {
                buttonState.Q<Image>().image = triggerOffGuiContent.image;
                Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
                triggerObject.gameObject.SetActive(false);
                EditorUtility.SetDirty(triggerObject.gameObject);
            }
            else
            {
                buttonState.Q<Image>().image = triggerOnGuiContent.image;
                Undo.RecordObject(triggerObject.gameObject, "Toggle Active");
                triggerObject.gameObject.SetActive(true);
                EditorUtility.SetDirty(triggerObject.gameObject);
            }
        };
        
        buttonState.Q<Image>().image = triggerObject.gameObject.activeSelf  ? triggerOnGuiContent.image : triggerOffGuiContent.image;

        return triggerElement;
    }
}
