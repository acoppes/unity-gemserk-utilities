using Gemserk.Actions;
using UnityEngine;

public class MyTriggerCondition : MonoBehaviour, ITrigger.ICondition
{
    public bool evaluate;
    
    public bool Evaluate()
    {
        return evaluate;
    }
}