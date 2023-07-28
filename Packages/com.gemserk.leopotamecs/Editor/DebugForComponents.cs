using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    public static class DebugForComponents
    {
        sealed class StateComponentInspector : EcsComponentInspectorTyped<StatesComponent> {
            public override bool OnGuiTyped (string label, ref StatesComponent states, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                // EditorGUILayout.LabelField ($"{nameof(StatesComponent)}", EditorStyles.boldLabel);
                
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField ($"Active States");
                
                EditorGUI.indentLevel++;
                foreach (var state in states.states)
                {
                    EditorGUILayout.LabelField($"{state.Value.name} || {state.Value.time:0.00} || {state.Value.updateCount}");
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                
                return true;
            }
        }
        
        // sealed class ConfigurationComponentInspector : EcsComponentInspectorTyped<ConfigurationComponent> {
        //     public override bool OnGuiTyped (string label, ref ConfigurationComponent configurationComponent, EcsEntityDebugView entityView)
        //     {
        //         if (entityView != null)
        //         {
        //             EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        //         }
        //         
        //         EditorGUILayout.LabelField ("CONFIG");
        //         return true;
        //     }
        // }
        
        sealed class DisabledComponentInspector : EcsComponentInspectorTyped<DisabledComponent> {
            public override bool OnGuiTyped (string label, ref DisabledComponent disabledComponent, EcsEntityDebugView entityView) {
                
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }

                if (GUILayout.Button("enable"))
                {
                    entityView.World.GetPool<EnableDisabledComponent>().Add(entityView.Entity);   
                }
                
                return true;
            }
        }
    }
}
