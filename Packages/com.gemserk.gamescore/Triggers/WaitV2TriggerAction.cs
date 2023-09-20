using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class WaitV2TriggerAction : TriggerAction
    {
        public enum ActionType
        {
            DeltaTime = 0,
            FixedUpdateFrames = 1,
            UnscaledDeltaTime = 2
        }

        public ActionType actionType;
        
        [ConditionalField(fieldToCheck:nameof(actionType), false, ActionType.DeltaTime, ActionType.UnscaledDeltaTime)]
        public float time;
        
        [ConditionalField(fieldToCheck:nameof(actionType), false, ActionType.FixedUpdateFrames)]
        public int fixedFrames;

        private float currentTime;

        private int currentFrame;
        
        public override string GetObjectName()
        {
            if (actionType == ActionType.DeltaTime)
            {
                return $"WaitTime({time})";
            } 
            
            if (actionType == ActionType.UnscaledDeltaTime)
            {
                return $"WaitTimeUnscaled({time})";
            } 
            
            return $"WaitFramesFixedUpdate({fixedFrames})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var dt = Time.deltaTime;
            
            if (actionType == ActionType.UnscaledDeltaTime)
            {
                dt = Time.unscaledDeltaTime;
            }
            
            currentTime += dt;

            if (actionType == ActionType.DeltaTime || actionType == ActionType.UnscaledDeltaTime)
            {
                if (currentTime < time)
                {
                    return ITrigger.ExecutionResult.Running;
                }

                currentTime = 0;
                return ITrigger.ExecutionResult.Completed;
            }
            else
            {
                if (currentTime > Time.fixedDeltaTime)
                {
                    currentTime -= Time.fixedDeltaTime;
                    currentFrame++;
                }

                if (currentFrame < fixedFrames)
                {
                    return ITrigger.ExecutionResult.Running;
                }

                currentFrame = 0;
                currentTime = 0;
                
                return ITrigger.ExecutionResult.Completed;
            }
        }
    }
}