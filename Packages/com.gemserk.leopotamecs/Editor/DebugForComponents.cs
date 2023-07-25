using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    public static class DebugForComponents
    {
        public static bool DebugStatesComponent(string label, StatesComponent states)
        {
            EditorGUILayout.LabelField (label, EditorStyles.boldLabel);
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

            return false;
        }
        
        public static bool DebugConfigurationComponent(string label, ConfigurationComponent configuration)
        {
            EditorGUILayout.LabelField (label, EditorStyles.boldLabel);
            configuration.configuredVersion = EditorGUILayout.IntField("Version", configuration.configuredVersion);
            return false;
        }
        
        sealed class StateComponentInspector : EcsComponentInspectorTyped<StatesComponent> {
            public override bool OnGuiTyped (string label, ref StatesComponent states, EcsEntityDebugView entityView)
            {
                return DebugStatesComponent(label, states);
                
                // EditorGUILayout.LabelField (label, EditorStyles.boldLabel);
                // // EditorGUILayout.LabelField ($"{nameof(StatesComponent)}", EditorStyles.boldLabel);
                //
                // EditorGUI.indentLevel++;
                // EditorGUILayout.LabelField ($"Active States");
                //
                // EditorGUI.indentLevel++;
                // foreach (var state in states.states)
                // {
                //     EditorGUILayout.LabelField($"{state.Value.name} || {state.Value.time:0.00} || {state.Value.updateCount}");
                // }
                // EditorGUI.indentLevel--;
                // EditorGUI.indentLevel--;
                //
                // return true;
            }
        }
        
        sealed class DisabledComponentInspector : EcsComponentInspectorTyped<DisabledComponent> {
            public override bool OnGuiTyped (string label, ref DisabledComponent disabledComponent, EcsEntityDebugView entityView) {
                
                EditorGUILayout.LabelField ($"{nameof(DisabledComponent)}", EditorStyles.boldLabel);

                if (GUILayout.Button("enable"))
                {
                    entityView.World.GetPool<EnableDisabledComponent>().Add(entityView.Entity);   
                }
                
                return true;
            }
        }
    }
}
