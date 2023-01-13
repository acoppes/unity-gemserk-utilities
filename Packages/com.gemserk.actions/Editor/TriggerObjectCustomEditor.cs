using UnityEditor;
using UnityEngine;

namespace Gemserk.Actions.Editor
{
    [CustomEditor(typeof(TriggerObject), true)]
    public class TriggerObjectCustomEditor : UnityEditor.Editor
    {
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
            
            if (GUILayout.Button("Trigger"))
            {
                triggerObject.QueueExecution();
            }
        }
    }
}
