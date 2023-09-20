using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Game.Components.Abilities
{
    public struct AbilitiesComponent : IEntityComponent
    {
        public static AbilitiesComponent Create()
        {
            return new AbilitiesComponent()
            {
                abilities = new List<Ability>(),
                abilityByName = new Dictionary<string, Ability>()
            };
        }
        
        public List<Ability> abilities;
        private IDictionary<string, Ability> abilityByName;
        
        public bool hasExecutingAbility;

        public void Add(Ability ability)
        {
            abilities.Add(ability);
            abilityByName[ability.name] = ability;
        }
        
        public Ability GetAbilityNoNullCheck(string name)
        {
            return abilityByName[name];
        }

        public bool HasAbility(string name)
        {
            return abilityByName.ContainsKey(name);
        }

        public Ability GetAbility(string name)
        {
            if (abilityByName.TryGetValue(name, out var value))
            {
                return value;
            }
            return null;
        }
    }
}