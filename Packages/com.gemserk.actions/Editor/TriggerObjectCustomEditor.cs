using UnityEditor;
using UnityEngine;

namespace Gemserk.Actions.Editor
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
                if (GUILayout.Button(type.Name))
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
            EditorGUILayout.IntField("Pending Executions", trigger.pendingExecutions);
            if (trigger.actions.Count > 0)
            {
                var actionObject = trigger.actions[trigger.executingAction] as MonoBehaviour;
                EditorGUILayout.ObjectField("Current Action", 
                    actionObject.gameObject, typeof(GameObject), true);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            // show events, conditions and actions
            
            DrawTriggerButtons("Events", eventTypes, triggerObject.eventsParent);
            DrawTriggerButtons("Conditions", conditionTypes, triggerObject.conditionsParent);
            DrawTriggerButtons("Actions", actionTypes, triggerObject.actionsParent);
            
            
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Trigger Execution"))
                {
                    triggerObject.QueueExecution();
                }
            }
        }
    }
}
