﻿using System;
using Gemserk.Triggers;
using UnityEngine;

[Obsolete]
public class MyObsoleteTriggerAction : TriggerAction
{
    public float value1;

    public override string GetObjectName()
    {
        return $"Debug({value1})";
    }

    public override ITrigger.ExecutionResult Execute(object activator = null)
    {
        Debug.Log($"VALUE: {value1}");
        return ITrigger.ExecutionResult.Completed;
    }
}