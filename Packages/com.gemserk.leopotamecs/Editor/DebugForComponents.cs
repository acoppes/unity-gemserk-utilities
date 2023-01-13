using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;

namespace Gemserk.Leopotam.Ecs.Editor
{
    public static class DebugForComponents
    {
        sealed class StateComponentInspector : EcsComponentInspectorTyped<StatesComponent> {
            public override bool OnGuiTyped (string label, ref StatesComponent states, EcsEntityDebugView entityView) {
                
                EditorGUILayout.LabelField ($"{nameof(StatesComponent)}", EditorStyles.boldLabel);
                
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
    }
}
