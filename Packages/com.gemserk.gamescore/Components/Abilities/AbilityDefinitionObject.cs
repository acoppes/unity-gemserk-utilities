using UnityEngine;

namespace Game.Components.Abilities
{
    public class AbilityDefinitionObject : MonoBehaviour
    {
        public AbilityDefinition abilityDefinition = new ();

        private void OnValidate()
        {
            gameObject.name = abilityDefinition.name;
        }
    }
}