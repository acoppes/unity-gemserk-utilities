using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite.UnityEditor;
using MyGame;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class DebugForComponents
    {
        sealed class NewStateComponentInspector : EcsComponentInspectorTyped<StatesComponentV2> {
            public override bool OnGuiTyped (string label, ref StatesComponentV2 stateses, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                
                EditorGUILayout.LabelField ("Active States");
                EditorGUILayout.LabelField ($"{Convert.ToString(stateses.statesBitmask, 2).PadLeft(16, '0')}");
                
                var typesAsset = stateses.typesAsset;

                if (typesAsset)
                {
                    var names = new List<string>();
                    typesAsset.GetMaskNames(stateses.statesBitmask, names);

                    for (var i = 0; i < stateses.states.Length; i++)
                    {
                        var state = stateses.states[i];
                        var hasState = stateses.HasState(i);

                        var stateName = typesAsset.GetTypeName(i);
                        
                        EditorGUILayout.BeginHorizontal();
                        if (!string.IsNullOrEmpty(stateName))
                        {
                            EditorGUILayout.LabelField(
                                $"{stateName} || t:{state.time:0.00} || d:{state.duration:0.0}  || {state.updateCount}");
                            
                            if (hasState)
                            {
                                if (GUILayout.Button("Exit"))
                                {
                                    stateses.Exit(i);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Enter"))
                                {
                                    stateses.Enter(i);
                                }
                            }
                           
                        }
                        else
                        {
                            if (hasState)
                            {
                                EditorGUILayout.LabelField(
                                    $"{i} || t:{state.time:0.0} || d:{state.duration:0.0} || {state.updateCount}");
                                if (GUILayout.Button("Exit"))
                                {
                                    stateses.Exit(i);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    for (var i = 0; i < stateses.states.Length; i++)
                    {
                        var state = stateses.states[i];
                        
                        EditorGUILayout.BeginHorizontal();
                        var hasState = stateses.HasState(i);
                        
                        
                        if (hasState)
                        {
                            EditorGUILayout.LabelField(
                                $"{i} || t:{state.time:0.0} || d:{state.duration:0.0} || {state.updateCount}");
                            if (GUILayout.Button("Exit"))
                            {
                                stateses.Exit(i);
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