using System.Linq;
using System.Reflection;
using Gemserk.Utilities;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [InitializeOnLoad]
    public static class TriggerEditorsTypeCaches
    {
        public static TypeCache.TypeCollection eventTypes;
        public static TypeCache.TypeCollection conditionTypes;
        public static TypeCache.TypeCollection actionTypes;

        static TriggerEditorsTypeCaches()
        {
            eventTypes = TypeCache.GetTypesDerivedFrom<TriggerEvent>();
            conditionTypes = TypeCache.GetTypesDerivedFrom<TriggerCondition>();
            actionTypes = TypeCache.GetTypesDerivedFrom<TriggerAction>();
        }
    }
    
    [CustomEditor(typeof(TriggerObject), true)]
    public class TriggerObjectCustomEditor : UnityEditor.Editor
    {
        public static void DrawTriggerButtons(string category, TypeCache.TypeCollection typesCollection, 
            Transform parent)
        {
            if (Application.isPlaying)
                return;
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField($"Add {category}");
            EditorGUILayout.BeginVertical();
            foreach (var type in typesCollection)
            {
                if (type.IsAbstract)
                    continue;

                var buttonName = type.Name;
                
                var attributes = type.GetCustomAttributes(typeof(TriggerEditorAttribute)).ToList();

                if (attributes.Count > 0)
                {
                    var editorAttribute = attributes[0] as TriggerEditorAttribute;
                    buttonName = editorAttribute.editorName;
                }
                else
                {
                    buttonName = buttonName.Replace("TriggerCondition", "");
                    buttonName = buttonName.Replace("TriggerEvent", "");
                    buttonName = buttonName.Replace("TriggerAction", "");
                }
                
                if (GUILayout.Button(buttonName))
                {
                    var newActionObject = new GameObject(type.Name);
                    newActionObject.AddComponent(type);
                    newActionObject.transform.SetParent(parent, false);
                    EditorGUIUtility.PingObject(newActionObject);
                    Selection.activeObject = newActionObject;
                }
            }

            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            var triggerObject = target as TriggerObject;

            var trigger = triggerObject.trigger;

            EditorGUILayout.BeginVertical();
            // show 
            EditorGUI.BeginDisabledGroup(true);
            
            EditorGUILayout.LabelField("State", trigger.State.ToString());
            EditorGUILayout.IntField("Pending Executions", trigger.pendingExecutions.Count);
            EditorGUILayout.IntField("Completed Executions", trigger.executionTimes);
            
            if (trigger.actions.Count > 0)
            {
                var actionObject = trigger.actions[trigger.executingAction] as MonoBehaviour;
                EditorGUILayout.ObjectField("Current Action", 
                    actionObject.gameObject, typeof(GameObject), true);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            // show events, conditions and actions

            var eventsParent = triggerObject.transform.FindOrCreateFolder("Events");
            var conditionsParent = triggerObject.transform.FindOrCreateFolder("Conditions");
            var actionsParent = triggerObject.transform.FindOrCreateFolder("Actions");
            
            DrawTriggerButtons("Events", TriggerEditorsTypeCaches.eventTypes, eventsParent);
            DrawTriggerButtons("Conditions", TriggerEditorsTypeCaches.conditionTypes, conditionsParent);
            DrawTriggerButtons("Actions", TriggerEditorsTypeCaches.actionTypes, actionsParent);
            
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Trigger (checks conditions)"))
                {
                    triggerObject.QueueExecution();
                }
                
                if (GUILayout.Button("Force Execution"))
                {
                    triggerObject.ForceQueueExecution();
                }
            }
        }
    }
    
    [CustomEditor(typeof(TriggerActionGroup), true)]
    public class TriggerActionGroupCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var triggerActionGroup = target as TriggerActionGroup;
            var actionsParent = triggerActionGroup.transform;
            TriggerObjectCustomEditor.DrawTriggerButtons("Actions", TriggerEditorsTypeCaches.actionTypes, actionsParent);
        }
    }
}
