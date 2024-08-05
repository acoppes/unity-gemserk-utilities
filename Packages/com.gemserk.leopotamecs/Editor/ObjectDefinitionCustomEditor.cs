using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ObjectEntityDefinition), true)]
    public class ObjectDefinitionCustomEditor : UnityEditor.Editor
    {
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<ComponentDefinitionBase>()
                .Where(t => !t.IsAbstract)
                .ToList();
            types.Sort(GuiUtilities.NameComparison);
        }

        public override void OnInspectorGUI()
        {
            var targetObject = target as ObjectEntityDefinition;

            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;
            
            DrawDefaultInspector();
            
            if (targetObject != null)
            {
                var components = targetObject
                    .GetComponents<IComponentDefinition>().ToList();

                GuiUtilities.DrawSelectTypesGui(targetObject.gameObject, types, components);
            }
        }
    }
}