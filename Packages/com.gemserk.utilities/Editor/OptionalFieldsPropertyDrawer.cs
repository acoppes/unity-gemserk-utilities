using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(OptionalFieldsAttribute))]
    public class OptionalFieldsPropertyDrawer : PropertyDrawer
    {
        private float propertyFieldHeight = 20;

        private struct FieldTuple
        {
            public string checkPropertyName;
            public string fieldPropertyName;
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var r1 = new Rect(position.x, position.y, position.width, propertyFieldHeight);
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
        
            var showNext = false;

            var lastCheckPropertyName = string.Empty;

            var enabledProperties = new List<FieldTuple>();
        
            var disabledProperties = new List<FieldTuple> { 
                new FieldTuple
                {
                    checkPropertyName = string.Empty,
                    fieldPropertyName = "Click to add",
                } 
            };
        
            var nextElement = property.Copy();
            var hasElement = nextElement.Next(true);
            
            while (hasElement)
            {
                // var prop = iterator.Current as SerializedProperty;
                var prop = nextElement;
            
                if (prop.name.ToLowerInvariant().StartsWith("check"))
                {
                    showNext = prop.boolValue;
                    lastCheckPropertyName = prop.name;

                    hasElement = nextElement.Next(false);
                    continue;
                }

                if (!showNext)
                {
                    if (lastCheckPropertyName != null)
                    {
                        disabledProperties.Add(new FieldTuple
                        {
                            checkPropertyName = lastCheckPropertyName,
                            fieldPropertyName = prop.name
                        });
                    }
                
                    lastCheckPropertyName = null;
                
                    hasElement = nextElement.Next(false);
                    continue;
                }

                if (showNext)
                {
                    if (lastCheckPropertyName != null)
                    {
                        enabledProperties.Add(new FieldTuple
                        {
                            checkPropertyName = lastCheckPropertyName,
                            fieldPropertyName = prop.name
                        });
                    }

                    lastCheckPropertyName = null;
                }
            
                hasElement = nextElement.Next(false);
            }
        
            r1.y += propertyFieldHeight;
        
            EditorGUI.BeginChangeCheck();
        
            var options = disabledProperties.Select(t => t.fieldPropertyName).ToArray();

            if (options.Length > 1)
            {
                var newSelectedIndex = EditorGUI.Popup(r1, 0, options);

                if (EditorGUI.EndChangeCheck())
                {
                    // add property to enable list by checking bool...
                    // reset options

                    if (newSelectedIndex > 0)
                    {
                        var tuple = disabledProperties[newSelectedIndex];

                        var checkProperty = property.FindPropertyRelative(tuple.checkPropertyName);
                        checkProperty.boolValue = true;
                    }
                }
            
                r1.y += propertyFieldHeight;
            }
            else
            {
                EditorGUI.Popup(r1, 0, new []{ "Nothing to add" });
                r1.y += propertyFieldHeight;
            }

            foreach (var tuple in enabledProperties)
            {
                // // Draw fields - passs GUIContent.none to each so they are drawn without labels
                var prop = property.FindPropertyRelative(tuple.fieldPropertyName);
            
                if (prop == null)
                    continue;

                var propRect = r1;
                var checkRect = r1;
            
                propRect.width -= 40;
            
                checkRect.x = checkRect.width - 30;
                checkRect.width = 30;

                EditorGUI.PropertyField(propRect, prop, true);
            
                var childrenPropertiesCount = prop.CountInProperty();
                if (childrenPropertiesCount > 1)
                    r1.y += propertyFieldHeight * (childrenPropertiesCount - 1);

                if (!EditorGUI.Toggle(checkRect, true))
                {
                    var checkProperty = property.FindPropertyRelative(tuple.checkPropertyName);
                    checkProperty.boolValue = false;
                }
            
                r1.y += propertyFieldHeight;
            }
        
            // Calculate rects
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var totalElements = 2;
            var iterator = property.Copy().GetEnumerator();
            var showNext = false;
        
            while (iterator.MoveNext())
            {
                var prop = iterator.Current as SerializedProperty;
                if (prop.name.ToLowerInvariant().StartsWith("check"))
                {
                    showNext = prop.boolValue;
                    continue;
                }
            
                if (!showNext)
                {
                    continue;
                }
            
                totalElements += prop.Copy().CountInProperty();
                showNext = false;
            }

            return totalElements * propertyFieldHeight;
        }
    }
}