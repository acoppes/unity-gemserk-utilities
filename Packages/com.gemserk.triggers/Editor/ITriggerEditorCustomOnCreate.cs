using Gemserk.Triggers.Actions;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [InitializeOnLoad]
    public static class IfThenElseCustomCreate
    {
        static IfThenElseCustomCreate()
        {
            TriggerSystemEditorWindow.customCreateHandlers[typeof(IfThenElseTriggerAction)] = CustomCreateHandler;
        }

        private static void CustomCreateHandler(GameObject gameObject, Component triggerComponent)
        {
            var ifThenElseTriggerAction = triggerComponent as IfThenElseTriggerAction;

            if (!ifThenElseTriggerAction)
            {
                return;
            }
            
            var conditionObject = new GameObject("If");
            var thenObject = new GameObject("Then");
            var elseObject = new GameObject("Else");
            
            Undo.SetTransformParent(conditionObject.transform, gameObject.transform, gameObject.name);
            Undo.SetTransformParent(thenObject.transform, gameObject.transform, gameObject.name);
            Undo.SetTransformParent(elseObject.transform, gameObject.transform, gameObject.name);

            ifThenElseTriggerAction.thenGroup = Undo.AddComponent(thenObject, typeof(TriggerActionGroup)) as TriggerActionGroup;
            ifThenElseTriggerAction.elseGroup = Undo.AddComponent(elseObject, typeof(TriggerActionGroup)) as TriggerActionGroup;

            if (ifThenElseTriggerAction.thenGroup) 
                ifThenElseTriggerAction.thenGroup.groupName = "Then";
            
            if (ifThenElseTriggerAction.elseGroup) 
                ifThenElseTriggerAction.elseGroup.groupName = "Else";
        }
    }
}