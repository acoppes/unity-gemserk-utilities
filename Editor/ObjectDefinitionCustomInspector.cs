using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ObjectEntityDefinition))]
    public class ObjectDefinitionCustomInspector : UnityEditor.Editor
    {
        private List<Type> entityComponentDefinitionTypes;
        
        private void OnEnable()
        {
            entityComponentDefinitionTypes = TypeCache.GetTypesDerivedFrom<IEntityComponentDefinition>().ToList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var objectEntityDefinition = target as ObjectEntityDefinition;

            // add buttons for each kind of ientitydefinitions serializable
            foreach (var type in entityComponentDefinitionTypes)
            {
                if (objectEntityDefinition.componentDefinitions
                        .Where(c => c != null)
                        .Count(c => c.GetType() == type) > 0)
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }
                
                if (GUILayout.Button($"Add {type.Name.Replace("Definition", "")}"))
                {
                    var componentDefinition = (IEntityComponentDefinition) Activator.CreateInstance(type);
                    objectEntityDefinition.componentDefinitions.Add(componentDefinition);
                    EditorUtility.SetDirty(objectEntityDefinition);
                    AssetDatabase.SaveAssetIfDirty(objectEntityDefinition);
                    
                }
            }
        }
    }
}