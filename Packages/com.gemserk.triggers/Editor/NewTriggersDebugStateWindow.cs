using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NewTriggersDebugStateWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    [SerializeField]
    private VisualTreeAsset m_VisualElementTemplate = default;

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

        var executeGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_PlayButton").image)
        {
            tooltip = "Queue Execution"
        };
        
        var forceExecuteGuiContent = new GUIContent(EditorGUIUtility.IconContent("d_StepButton").image)
        {
            tooltip = "Force Execution"
        };
        
        var expandGuiContent = new GUIContent(EditorGUIUtility.IconContent("FolderOpened On Icon").image)
        {
            tooltip = "Toggle Expand"
        };
        
        for (int i = 0; i < 50; i++)
        {
            var custom = m_VisualElementTemplate.Instantiate();

            var element = custom.Q("TriggerElement");
            
            if (i % 2 == 0)
            {
                element.AddToClassList("trigger-even");
            }

            // custom.Q<Foldout>().value = false;
            
            element.Q<Label>().text = "MY CUSTOM TEXT 1";
            var buttonExecute = element.Q<Button>("ButtonExecute");
            buttonExecute.clicked += () =>
            {
                Debug.Log("EXECUTE");
            };
            buttonExecute.Q<Image>().image = executeGuiContent.image;
            
            var buttonForceExecute = element.Q<Button>("ButtonForceExecute");
            buttonForceExecute.clicked += () =>
            {
                Debug.Log("FORCE EXECUTE");
            };
            buttonForceExecute.Q<Image>().image = forceExecuteGuiContent.image; 
            
            var buttonExpand = element.Q<Button>("ButtonExpand");
            buttonExpand.clicked += () =>
            {
                Debug.Log("EXPAND ON/OFF");
            };
            buttonExpand.Q<Image>().image = expandGuiContent.image; 
            
            mainScroll.Add(custom);
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
}
