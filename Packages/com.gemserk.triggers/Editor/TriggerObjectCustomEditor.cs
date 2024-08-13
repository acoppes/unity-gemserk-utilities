using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
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

            {
                EditorGUI.BeginChangeCheck();
                triggerObject.executionType =
                    (TriggerObject.ExecutionType)EditorGUILayout.EnumPopup("Max Executions",
                        triggerObject.executionType);

                if (triggerObject.executionType == TriggerObject.ExecutionType.More)
                {
                    triggerObject.maxExecutions = EditorGUILayout.IntField(triggerObject.maxExecutions);
                    triggerObject.maxExecutions = Mathf.Clamp(triggerObject.maxExecutions, 1, 9999);
                }

                if (EditorGUI.EndChangeCheck())
                {   
                    EditorUtility.SetDirty(triggerObject);
                }
            }
            
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
        }
    }
}
