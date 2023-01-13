using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Editor
{
    [CustomEditor(typeof(EntityReference))]
    [CanEditMultipleObjects]
    public class EntityReferenceInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var entityReference = target as EntityReference;
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Entity" ,$"{entityReference.entity.entity:x8}");
            EditorGUILayout.LabelField("Generation" ,$"{entityReference.entity.generation}");
            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Select"))
            {
                var debugViews = FindObjectsOfType<EcsEntityDebugView>();
                foreach (var debugView in debugViews)
                {
                    if (debugView.Entity == entityReference.entity.entity)
                    {
                        EditorGUIUtility.PingObject(debugView.gameObject);
                        Selection.activeObject = debugView.gameObject;
                        break;
                    }
                }
            }
        }
    }
}