using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public static class GuiUtilities
    {
        public static int NameComparison(Type a, Type b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
        }
        
        [Obsolete("Use the SerializedObject version.")]
        public static void DrawSelectTypesGui<T>(GameObject gameObject, 
            IEnumerable<Type> types, IEnumerable<T> excludeComponents, string[] cleanupFilter = null) where T : class
        {
            var addedTypes = excludeComponents.Select(c => c.GetType())
                .ToList();
            
            // addedTypes.Sort(NameComparison);
            
            var addTypes = types.Except(addedTypes).ToList();

            if (addTypes.Count > 0)
            {
                var typeNames = new List<string>(new[] { "<< SELECT TO ADD >>" });

                var renamedTypes = addTypes.Select(t => t.Name);
                    
                if (cleanupFilter != null)
                {
                    foreach (var filter in cleanupFilter)
                    {
                        renamedTypes = renamedTypes.Select(t => t.Replace(filter, "")).ToList();
                    }
                }
                
                typeNames.AddRange(renamedTypes);

                var selected = 0;
                EditorGUI.BeginChangeCheck();
                selected = EditorGUILayout.Popup(selected, typeNames.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    var typeToAdd = addTypes[selected - 1];
                    Undo.AddComponent(gameObject, typeToAdd);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Popup(0, new []{ "<< NO ELEMENTS TO ADD >>"});
                EditorGUI.EndDisabledGroup();
            }
        }
        
        public static void DrawSelectTypesGui<T>(SerializedObject serializedObject, 
            IEnumerable<Type> types, string[] cleanupFilter = null) where T : class
        {
            var excludeComponents = new List<T>();
                    
            if (serializedObject.isEditingMultipleObjects)
            {
                // TODO: decide what we want here, not bad case is showing everything
                // best case is to not show components that are in all objects.
                // and also don't add twice same component (if already there)
            }
            else
            {
                var component = serializedObject.targetObject as Component;
                if (component)
                {
                    excludeComponents = component.GetComponents<T>().ToList();
                }
            }
            
            var addedTypes = excludeComponents.Select(c => c.GetType())
                .ToList();
            
            var addTypes = types.Except(addedTypes).ToList();

            if (addTypes.Count > 0)
            {
                var typeNames = new List<string>(new[] { "<< SELECT TO ADD >>" });

                var renamedTypes = addTypes.Select(t => t.Name);
                    
                if (cleanupFilter != null)
                {
                    foreach (var filter in cleanupFilter)
                    {
                        renamedTypes = renamedTypes.Select(t => t.Replace(filter, "")).ToList();
                    }
                }
                
                typeNames.AddRange(renamedTypes);

                var selected = 0;
                EditorGUI.BeginChangeCheck();
                selected = EditorGUILayout.Popup(selected, typeNames.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    var typeToAdd = addTypes[selected - 1];
                    
                    if (serializedObject.isEditingMultipleObjects)
                    {
                        foreach (var targetObject in serializedObject.targetObjects)
                        {
                            var component = targetObject as Component;
                            if (component)
                            {
                                Undo.AddComponent(component.gameObject, typeToAdd);
                            }
                        }
                    }
                    else
                    {
                        var component = serializedObject.targetObject as Component;
                        if (component)
                        {
                            Undo.AddComponent(component.gameObject, typeToAdd);
                        }
                    }
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Popup(0, new []{ "<< NO ELEMENTS TO ADD >>"});
                EditorGUI.EndDisabledGroup();
            }
        }


    }
}