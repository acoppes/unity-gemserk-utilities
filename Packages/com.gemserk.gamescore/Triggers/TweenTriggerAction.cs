using System;
using Gemserk.Triggers;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Triggers
{
    public class TweenTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Opacity = 0,
            Scale = 1
        }

        public ActionType actionType= ActionType.Opacity;

        public GameObject target;
        public SpriteRenderer fadeObject;

        public LeanTweenConfiguration tween;

        public override string GetObjectName()
        {
            if (actionType == ActionType.Scale)
            {
                if (target != null)
                {
                    return $"TweenScale({target.name}, {tween.time}, {tween.from.x}, {tween.to.x}, {tween.easing})";
                }
                return $"TweenScale(undefined, {tween.time}, {tween.from.x}, {tween.to.x}, {tween.easing})";
            }
            
            if (fadeObject != null)
            {
                return $"TweenSpriteAlpha({fadeObject.name}, {tween.time}, {tween.from.x}, {tween.to.x}, {tween.easing})";
            }
            return $"TweenSpriteAlpha({tween.time}, {tween.from.x}, {tween.to.x}, {tween.easing})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (target != null && fadeObject == null)
            {
                fadeObject = target.GetComponent<SpriteRenderer>();
            }

            if (fadeObject != null && target == null)
            {
                target = fadeObject.gameObject;
            }

            if (actionType == ActionType.Opacity)
            {
                // if (fadeObject == null)
                // {
                //     throw new Exception("Invalid gameobject, missing sprite renderer");
                // }

                // var tweenInstance = LeanTween.value(target, tween.from, tween.to, tween.time)
                //     .setEase(tween.easing)
                //     .setUseEstimatedTime(tween.useEstimatedTime)
                //     .setOnUpdate(OnSpriteUpdate);

                LTDescr tweenInstance;
                
                if (fadeObject != null)
                {
                    tweenInstance = LeanTween.value(fadeObject.gameObject, tween.from, tween.to, tween.time)
                        .setEase(tween.easing)
                        .setUseEstimatedTime(tween.useEstimatedTime)
                        .setOnUpdate(OnSpriteUpdate);
                } else
                {
                    var rectTransform = target.GetComponent<RectTransform>();
                    if (rectTransform)
                    {
                        tweenInstance = LeanTween.alpha(target.GetComponent<RectTransform>(), tween.to.x, tween.time)
                            .setFrom(tween.from.x)
                            .setEase(tween.easing)
                            .setUseEstimatedTime(tween.useEstimatedTime);
                    }
                    else
                    {
                        tweenInstance = LeanTween.alpha(target, tween.to.x, tween.time)
                            .setFrom(tween.from.x)
                            .setEase(tween.easing)
                            .setUseEstimatedTime(tween.useEstimatedTime);
                    }
                }
                
                if (tween.loopCount != 0)
                {
                    tweenInstance = tweenInstance.setLoopCount(tween.loopCount);
                } 

                if (tween.pingPong)
                {
                    tweenInstance = tweenInstance.setLoopPingPong();
                }
                
                // if (!tween.useCurrentValueForFrom)
                // {
                //     tweenInstance.setFrom(tween.from);
                // }

            } else if (actionType == ActionType.Scale)
            {
                if (target == null)
                {
                    throw new Exception("Invalid gameobject, missing sprite renderer");
                }

                var tweenInstance = LeanTween.scale(target, tween.to, tween.time)
                    .setEase(tween.easing)
                    .setUseEstimatedTime(tween.useEstimatedTime);

                if (tween.loopCount != 0)
                {
                    tweenInstance = tweenInstance.setLoopCount(tween.loopCount);
                } 

                if (tween.pingPong)
                {
                    tweenInstance = tweenInstance.setLoopPingPong();
                }
                
                if (!tween.useCurrentValueForFrom)
                {
                    tweenInstance.setFrom(tween.from);
                }
            }
            
            // if (tween.time <= 0.001f)
            // {
            //     fadeObject.color = fadeObject.color.WithAlphaSetTo(tween.to.x);
            // }
            // else
            // {
            //
            // }
            
            return ITrigger.ExecutionResult.Completed;
        }

        private void OnSpriteUpdate(float alpha)
        {
            fadeObject.color = fadeObject.color.WithAlphaSetTo(alpha);
        }

    }
}