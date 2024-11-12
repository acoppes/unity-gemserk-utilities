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
                .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)))
                .ToList();
            types.Sort(GuiUtilities.NameComparison);
        }
        
        public override void OnInspectorGUI()
        {
            var entityPrefabInstance = target as BaseEntityPrefabInstance;

            DrawDefaultInspector();
            
            if (!Application.isPlaying)
            {
                // GuiUtilities.DrawSelectTypesGui<IEntityInstanceParameter>(serializedObject, types, new []
                // {
                //     "ComponentDefinition",
                //     "InstanceParameter"
                // });
                
                if (GUILayout.Button("<< SELECT TO ADD >>"))
                {
                    var rect = EditorGUILayout.GetControlRect();
                    // var addTypes = 
                    //     GuiUtilities.FilterAddedComponents<IComponentDefinition>(serializedObject, types);
                    PopupWindow.Show(rect, new AddComponentPopup()
                    {
                        types = types,
                        serializedObject = serializedObject,
                        cleanupFilter = new []
                        {
                            "ComponentDefinition",
                            "InstanceParameter"
                        },
                    });
                }
            }
            else
            {
                if (GUILayout.Button("Instantiate"))
                {
                    entityPrefabInstance?.InstantiateEntity();
                }
            }
        }
    }
}