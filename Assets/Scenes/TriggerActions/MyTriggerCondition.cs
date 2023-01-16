using Gemserk.Actions;

public class MyTriggerCondition : TriggerCondition
{
    public bool evaluate;
    
    public override bool Evaluate(object activator = null)
    {
        return evaluate;
    }
}