using UnityEngine;

namespace Gemserk.Actions
{
    public class TriggerObject : MonoBehaviour, ITrigger
    {
        [SerializeReference]
        [SubclassSelector]
        public IAction action;
    }
}