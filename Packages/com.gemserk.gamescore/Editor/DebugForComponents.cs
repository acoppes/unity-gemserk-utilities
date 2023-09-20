using Game.Components;
using Game.Components.Abilities;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class DebugForComponents
    {
        sealed class AbilitiesComponentInspector : EcsComponentInspectorTyped<AbilitiesComponent> {
            public override bool OnGuiTyped (string label, ref AbilitiesComponent abilities, EcsEntityDebugView entityView) {
                
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;
                foreach (var ability in abilities.abilities)
                {
                    EditorGUILayout.LabelField(ability.name);
                    EditorGUI.indentLevel++;
                    
                    EditorGUILayout.Toggle("Is Ready", ability.isReady);

                    EditorGUILayout.Toggle("Running", ability.isExecuting);
                    EditorGUILayout.Toggle("Pending", ability.pendingExecution);
                    EditorGUILayout.LabelField("Charges", $"{ability.currentCharges} / {ability.totalCharges}");
                    EditorGUILayout.LabelField("Cooldown",$"{ability.cooldown.current:0.0}/{ability.cooldown.Total:0.0}");

                    if (ability.hasTargets)
                    {
                        EditorGUILayout.LabelField("-- Targets --");
                        EditorGUI.indentLevel++;
                        foreach (var abilityTarget in ability.abilityTargets)
                        {
                            EditorGUILayout.LabelField(abilityTarget.ToString());
                        }
                        EditorGUI.indentLevel--;
                    }
                 
                    

                    
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                
                return true;
            }
        }
        
        sealed class ControlComponentInspector : EcsComponentInspectorTyped<InputComponent> {
            public override bool OnGuiTyped (string label, ref InputComponent inputComponent, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;

                Vector2 val = EditorGUILayout.Vector2Field("direction", 
                    inputComponent.direction().vector2);
                inputComponent.direction().vector2 = val;

                var buttons = inputComponent.actions;
                foreach (var buttonName in buttons.Keys)
                {
                    var button = buttons[buttonName];
                    EditorGUILayout.BeginHorizontal();

                    if (button.type == 1)
                    {
                        button.isPressed = EditorGUILayout.Toggle(buttonName, button.isPressed);
                    } else if (button.type == 0)
                    {
                        button.vector2 = EditorGUILayout.Vector2Field(buttonName, button.vector2);
                    }
                    
                    // EditorGUILayout.Toggle("wasPressed", button.wasPressed);
                    EditorGUILayout.EndHorizontal();
                }

                // EditorGUI.BeginDisabledGroup(true);
                // EditorGUILayout.Toggle(nameof(ControlComponent.forward), controlComponent.forward.isPressed);
                // EditorGUILayout.Toggle(nameof(ControlComponent.backward), controlComponent.backward.isPressed);
                // EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel--;
                
                return true;
            }
        }
        
        sealed class BufferedInputComponentInspector : EcsComponentInspectorTyped<BufferedInputComponent> {
            public override bool OnGuiTyped (string label, ref BufferedInputComponent bufferedInput, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;
                foreach (var action in bufferedInput.buffer)
                {
                    EditorGUILayout.LabelField(action);
                }
                EditorGUI.indentLevel--;
                return true;
            }
        }
        
        sealed class AttachPointsComponentInspector : EcsComponentInspectorTyped<AttachPointsComponent> {
            public override bool OnGuiTyped (string label, ref AttachPointsComponent attachPoints, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;

                foreach (var key in attachPoints.attachPoints.Keys)
                {
                    var attachPoint = attachPoints.attachPoints[key];
                    
                    EditorGUILayout.LabelField(key);
                    EditorGUI.indentLevel++;
                    
                    // EditorGUILayout.BeginHorizontal();
                    attachPoint.localPosition = EditorGUILayout.Vector3Field("offset", attachPoint.localPosition);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Vector3Field("position", attachPoint.position);
                    EditorGUI.EndDisabledGroup();
                    // EditorGUILayout.Toggle("wasPressed", button.wasPressed);
                    // EditorGUILayout.EndHorizontal();
                    
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                
                return true;
            }
        }
        
        sealed class TypesComponentInspector : EcsComponentInspectorTyped<TypesComponent> {
            public override bool OnGuiTyped (string label, ref TypesComponent types, EcsEntityDebugView entityView)
            {
                if (entityView != null)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;

                foreach (var type in types.types)
                {
                    EditorGUILayout.LabelField(type);
                }

                EditorGUI.indentLevel--;
                
                return true;
            }
        }
    }
}