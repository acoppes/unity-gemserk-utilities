using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(BaseEntityPrefabInstance), true)]
    [CanEditMultipleObjects]
    public class EntityPrefabInstanceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var entityPrefabInstance = target as BaseEntityPrefabInstance;

            DrawDefaultInspector();

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Instantiate"))
                {
                    if (entityPrefabInstance != null)
                    {
                        entityPrefabInstance.InstantiateEntity();
                    }
                }
            }
        }
    }
}