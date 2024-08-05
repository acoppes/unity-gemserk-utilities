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
        
        public static void DrawSelectTypesGui<T>(GameObject gameObject, 
            IEnumerable<Type> types, IEnumerable<T> components) where T : class
        {
            var addedTypes = components.Select(c => c.GetType())
                .ToList();
            
            // addedTypes.Sort(NameComparison);
            
            var addTypes = types.Except(addedTypes).ToList();

            if (addTypes.Count > 0)
            {
                var typeNames = new List<string>(new[] { "<< SELECT TO ADD >>" });
                typeNames.AddRange(addTypes.Select(t => t.Name.Replace("Definition", "")));

                var selected = 0;
                EditorGUI.BeginChangeCheck();
                selected = EditorGUILayout.Popup(selected, typeNames.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    var typeToAdd = addTypes[selected - 1];
                    gameObject.AddComponent(typeToAdd);
                    EditorUtility.SetDirty(gameObject);
                    AssetDatabase.SaveAssetIfDirty(gameObject);
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