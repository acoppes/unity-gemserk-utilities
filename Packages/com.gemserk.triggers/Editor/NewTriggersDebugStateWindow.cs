using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NewTriggersDebugStateWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    [SerializeField]
    private VisualTreeAsset m_VisualElementTemplate = default;

    // [MenuItem("Window/UI Toolkit/NewTriggersDebugStateWindow")]
    // public static void ShowExample()
    // {
    //     var wnd = GetWindow<NewTriggersDebugStateWindow>();
    //     wnd.titleContent = new GUIContent("NewTriggersDebugStateWindow");
    // }

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

        for (int i = 0; i < 50; i++)
        {
            var custom = m_VisualElementTemplate.Instantiate();
            if (i % 2 == 0)
            {
                custom.Q("TriggerElement").AddToClassList("trigger-even");
            }

            custom.Q<Foldout>().value = false;
            custom.Q<Label>().text = "MY CUSTOM TEXT 1";
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
