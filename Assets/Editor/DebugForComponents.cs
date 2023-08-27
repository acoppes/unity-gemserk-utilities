using System;
using System.Collections.Generic;
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
                
                
                EditorGUILayout.LabelField ("Active States");
                EditorGUILayout.LabelField ($"{Convert.ToString(states.statesBitmask, 2).PadLeft(16, '0')}");
                
                var typesAsset = states.typesAsset;

                if (typesAsset)
                {
                    var names = new List<string>();
                    typesAsset.GetMaskNames(states.statesBitmask, names);

                    for (var i = 0; i < states.states.Length; i++)
                    {
                        var state = states.states[i];
                        var hasState = states.HasState(i);

                        var stateName = typesAsset.GetTypeName(i);
                        
                        EditorGUILayout.BeginHorizontal();
                        if (!string.IsNullOrEmpty(stateName))
                        {
                            EditorGUILayout.LabelField(
                                $"{stateName} || {state.time:0.00} || {state.updateCount}");
                            
                            if (hasState)
                            {
                                if (GUILayout.Button("Exit"))
                                {
                                    states.Exit(i);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Enter"))
                                {
                                    states.Enter(i);
                                }
                            }
                           
                        }
                        else
                        {
                            if (hasState)
                            {
                                EditorGUILayout.LabelField(
                                    $"{i} || {state.time:0.00} || {state.updateCount}");
                                if (GUILayout.Button("Exit"))
                                {
                                    states.Exit(i);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    for (var i = 0; i < states.states.Length; i++)
                    {
                        var state = states.states[i];
                        
                        EditorGUILayout.BeginHorizontal();
                        var hasState = states.HasState(i);
                        
                        if (hasState)
                        {
                            EditorGUILayout.LabelField(
                                $"{i} || {state.time:0.00} || {state.updateCount}");
                            if (GUILayout.Button("Exit"))
                            {
                                states.Exit(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    
                }
                
                return true;
            }
        }
    }
}