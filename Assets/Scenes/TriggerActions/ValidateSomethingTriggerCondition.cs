using Gemserk.Triggers;

public class ValidateSomethingTriggerCondition : TriggerCondition
{
    public bool evaluate;

    public override string GetObjectName()
    {
        var str = evaluate ? "True" : "False";
        return str;
    }

    public override bool Evaluate(object activator = null)
    {
        return evaluate;
    }
}