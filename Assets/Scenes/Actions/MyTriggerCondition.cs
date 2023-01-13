using Gemserk.Actions;
using UnityEngine;

public class MyTriggerCondition : TriggerCondition
{
    public bool evaluate;
    
    public override bool Evaluate()
    {
        return evaluate;
    }
}