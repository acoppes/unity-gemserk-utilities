using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(BaseEntityPrefabInstance), true)]
    [CanEditMultipleObjects]
    public class EntityPrefabInstanceEditor : UnityEditor.Editor
    {
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<IEntityInstanceParameter>()
                .Where(t => !t.IsAbstract)
                .ToList();
            types.Sort(GuiUtilities.NameComparison);
        }
        
        public override void OnInspectorGUI()
        {
            var entityPrefabInstance = target as BaseEntityPrefabInstance;

            DrawDefaultInspector();
            
            if (entityPrefabInstance != null)
            {
                if (!Application.isPlaying)
                {
                    var components = entityPrefabInstance
                        .GetComponents<IEntityInstanceParameter>().ToList();

                    GuiUtilities.DrawSelectTypesGui(entityPrefabInstance.gameObject,
                        types, components);
                }
                else
                {
                    if (GUILayout.Button("Instantiate"))
                    {
                        entityPrefabInstance.InstantiateEntity();
                    }
                }
            }


        }
    }
}