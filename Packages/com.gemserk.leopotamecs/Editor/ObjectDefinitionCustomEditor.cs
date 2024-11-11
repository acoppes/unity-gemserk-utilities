using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ObjectEntityDefinition), true)]
    [CanEditMultipleObjects]
    public class ObjectDefinitionCustomEditor : UnityEditor.Editor
    {
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<IComponentDefinition>()
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)))
                .ToList();
            types.Sort(GuiUtilities.NameComparison);
        }

        public override void OnInspectorGUI()
        {
            // var targetObject = target as ObjectEntityDefinition;

            // var style = new GUIStyle(GUI.skin.label);
            // style.alignment = TextAnchor.MiddleCenter;
            // style.normal.textColor = Color.grey;
            
            DrawDefaultInspector();

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
            
            // GuiUtilities.DrawSelectTypesGui<IComponentDefinition>(serializedObject, types, new []
            // {
            //     "ComponentDefinition",
            //     "InstanceParameter"
            // });
        }
    }
}