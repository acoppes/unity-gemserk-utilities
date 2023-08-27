using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite.UnityEditor;
using MyGame;
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
        
        sealed class StatesComponentV2Inspector : EcsComponentInspectorTyped<StatesComponentV2> {
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
                                $"{stateName} || {state.time:0.00} || {state.updateCount}");
                            
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
                                    $"{i} || {state.time:0.00} || {state.updateCount}");
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
                                $"{i} || {state.time:0.00} || {state.updateCount}");
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
