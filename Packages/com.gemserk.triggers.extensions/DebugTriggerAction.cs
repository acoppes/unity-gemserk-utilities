﻿using UnityEngine;

namespace Gemserk.Triggers
{
    public class DebugTriggerAction : TriggerAction
    {
        [TextArea(1, 15)]
        public string text;
        
        public override string GetObjectName()
        {
            return "DebugLog()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            Debug.Log(string.Format(text, activator));
            return ITrigger.ExecutionResult.Completed;
        }
    }
}