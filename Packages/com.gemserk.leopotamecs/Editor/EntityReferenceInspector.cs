using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    #if GEMSERK_DEBUG_ENTITYREFERENCE && UNITY_EDITOR
    [CustomEditor(typeof(EntityReference))]
    [CanEditMultipleObjects]
    public class EntityReferenceInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var entityReference = target as EntityReference;
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Entity" ,$"{entityReference.entity.ecsEntity:x8}");
            EditorGUILayout.LabelField("Generation" ,$"{entityReference.entity.ecsGeneration}");
            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Select"))
            {
                var debugViews = FindObjectsOfType<EcsEntityDebugView>();
                foreach (var debugView in debugViews)
                {
                    if (debugView.Entity == entityReference.entity.ecsEntity)
                    {
                        EditorGUIUtility.PingObject(debugView.gameObject);
                        Selection.activeObject = debugView.gameObject;
                        break;
                    }
                }
            }
        }
    }
    #endif
}