using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers.Conditions
{
    public class IsUnityEditorTriggerCondition : TriggerCondition
    {
        public override string GetObjectName()
        {
            return "IsUnityEditor()";
        }

        public override bool Evaluate(object activator = null)
        {
            return Application.isEditor;
        }
    }
}