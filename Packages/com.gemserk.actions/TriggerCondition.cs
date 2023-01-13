using UnityEngine;

namespace Gemserk.Actions
{
    public abstract class TriggerCondition : MonoBehaviour, ITrigger.ICondition
    {
        public abstract bool Evaluate();
    }
}