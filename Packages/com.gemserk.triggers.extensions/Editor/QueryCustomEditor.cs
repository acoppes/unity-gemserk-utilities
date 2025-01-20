using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [CustomEditor(typeof(Query), true)]
    public class QueryCustomEditor : UnityEditor.Editor
    {
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<QueryParameterBase>()
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)))
                .ToList();
            types.Sort(GuiUtilities.NameComparison);
        }

        public override void OnInspectorGUI()
        {
            var query = target as Query;

            if (!query)
            {   
                DrawDefaultInspector();
                return;
            }
            
            // fix object name
            if (!query.disableEditorAutoName)
            {
                query.gameObject.name = $"Q({query.GetEntityQuery()})";
            }
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            
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
                        "QueryParameter",
                    },
                });
            }
            
            // GuiUtilities.DrawSelectTypesGui<QueryParameterBase>(serializedObject, types, new []
            // {
            //     "QueryParameter",
            // });
        }
    }
    
}