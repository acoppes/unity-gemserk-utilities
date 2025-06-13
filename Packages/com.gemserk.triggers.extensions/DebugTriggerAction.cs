using System;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class DebugTriggerAction : TriggerAction
    {
        [TextArea(1, 15)]
        public string text;

        public LogType logType = LogType.Log;
        
        public override string GetObjectName()
        {
            return $"Debug{logType}()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(string.Format(text, activator));
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(string.Format(text, activator));
                    break;
                case LogType.Warning:
                    Debug.LogWarning(string.Format(text, activator));
                    break;
                case LogType.Log:
                    Debug.Log(string.Format(text, activator));
                    break;
                case LogType.Exception:
                    Debug.LogException(new Exception(string.Format(text, activator)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}