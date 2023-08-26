using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite.UnityEditor;
using MyGame;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class DebugForComponents
    {
        sealed class NewStateComponentInspector : EcsComponentInspectorTyped<NewStateComponent> {
            public override bool OnGuiTyped (string label, ref NewStateComponent states, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField ($"Active States");

                EditorGUI.indentLevel++;
                var typesAsset = states.typesAsset;

                if (typesAsset)
                {
                    var names = new List<string>();
                    typesAsset.GetMaskNames(states.statesBitmask, names);

                    for (var i = 0; i < states.states.Length; i++)
                    {
                        var state = states.states[i];
                        if (states.HasState(1 << i))
                        {
                            EditorGUILayout.LabelField(
                                $"{typesAsset.GetTypeName(1 << i)} || {state.time:0.00} || {state.updateCount}");
                        }
                    }
                }
                else
                {
                    EditorGUILayout.LabelField($"{states.statesBitmask}, {states.subStatesBitmask}");
                }
                
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                
                return true;
            }
        }
    }
}