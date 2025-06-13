using System;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class ThrowExceptionTriggerAction : TriggerAction
    {
        [TextArea(1, 15)]
        public string text;

        public override string GetObjectName()
        {
            return "ThrowExceptionWithMessage()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            throw new Exception(string.Format(text, activator));
        }
    }
}