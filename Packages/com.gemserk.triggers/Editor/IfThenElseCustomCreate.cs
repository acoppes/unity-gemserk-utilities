using System;
using Gemserk.Triggers.Actions;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    public interface ITriggerCreateHandler
    {
        bool CanHandle(Type type);

        void HandleCreate(GameObject gameObject, Component triggerComponent);
    }
    
    [InitializeOnLoad]
    public static class IfThenElseCustomCreate
    {
        static IfThenElseCustomCreate()
        {
            TriggerSystemEditorWindow.triggerCreateHandlers.Add(new IfThenElseCreator());
        }
        
        private class IfThenElseCreator : ITriggerCreateHandler
        {
            public bool CanHandle(Type type)
            {
                if (type.IsSubclassOf(typeof(TriggerCondition)))
                    return true;

                return type == typeof(IfThenElseTriggerAction);
            }

            public void HandleCreate(GameObject gameObject, Component triggerComponent)
            {
                if (triggerComponent is IfThenElseTriggerAction ifThenElseTriggerAction)
                {
                    // var conditionObject = new GameObject("If");
                    var thenObject = new GameObject("Then");
                    var elseObject = new GameObject("Else");
            
                    // Undo.SetTransformParent(conditionObject.transform, gameObject.transform, gameObject.name);
                    Undo.SetTransformParent(thenObject.transform, gameObject.transform, gameObject.name);
                    Undo.SetTransformParent(elseObject.transform, gameObject.transform, gameObject.name);
            
                    ifThenElseTriggerAction.thenGroup = Undo.AddComponent(thenObject, typeof(TriggerActionGroup)) as TriggerActionGroup;
                    ifThenElseTriggerAction.elseGroup = Undo.AddComponent(elseObject, typeof(TriggerActionGroup)) as TriggerActionGroup;

                    if (ifThenElseTriggerAction.thenGroup) 
                        ifThenElseTriggerAction.thenGroup.groupName = "Then";
            
                    if (ifThenElseTriggerAction.elseGroup) 
                        ifThenElseTriggerAction.elseGroup.groupName = "Else";
                    
                    EditorUtility.SetDirty(ifThenElseTriggerAction);
                    EditorUtility.SetDirty(ifThenElseTriggerAction.gameObject);
                }

                if (triggerComponent is TriggerCondition triggerCondition)
                {
                    if (gameObject.transform.parent)
                    {
                        var parentIfThenElse = gameObject.transform.parent.GetComponent<IfThenElseTriggerAction>();
                        if (parentIfThenElse)
                        {
                            parentIfThenElse.condition = triggerCondition;
                            gameObject.transform.SetAsFirstSibling();
                            EditorUtility.SetDirty(parentIfThenElse);
                            EditorUtility.SetDirty(parentIfThenElse.gameObject);
                        }
                    }
                }

            }
        }
    }
}