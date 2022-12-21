using Gemserk.Leopotam.Ecs;
using UnityEditor;

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
        }
    }
}