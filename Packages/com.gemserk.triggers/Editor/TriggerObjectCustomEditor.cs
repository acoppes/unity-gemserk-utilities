using System.Linq;
using System.Reflection;
using Gemserk.Utilities;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [CustomEditor(typeof(TriggerObject), true)]
    public class TriggerObjectCustomEditor : UnityEditor.Editor
    {
        private TypeCache.TypeCollection eventTypes;
        private TypeCache.TypeCollection conditionTypes;
        private TypeCache.TypeCollection actionTypes;
        
        private void OnEnable()
        {
            eventTypes = TypeCache.GetTypesDerivedFrom<TriggerEvent>();
            conditionTypes = TypeCache.GetTypesDerivedFrom<TriggerCondition>();
            actionTypes = TypeCache.GetTypesDerivedFrom<TriggerAction>();
        }

        private static void DrawTriggerButtons(string category, TypeCache.TypeCollection typesCollection, 
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
                    newActionObject.transform.SetParent(parent);
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
            
            DrawTriggerButtons("Events", eventTypes, eventsParent);
            DrawTriggerButtons("Conditions", conditionTypes, conditionsParent);
            DrawTriggerButtons("Actions", actionTypes, actionsParent);
            
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
}
