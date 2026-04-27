using UnityEngine;

namespace Game.Components.Abilities
{
    public class AbilityDefinitionObject : MonoBehaviour
    {
        public AbilityDefinition abilityDefinition;

        private void OnValidate()
        {
            gameObject.name = abilityDefinition.name;
        }
    }
}