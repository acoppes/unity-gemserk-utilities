using Gemserk.Leopotam.Gameplay.Controllers;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;

namespace Gemserk.Leopotam.Gameplay.Editor
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
                    EditorGUILayout.LabelField($"{state.Key}");
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;

                return true;
            }
        }
    }
}
